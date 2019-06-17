using AutoMapper;
using DataDesafio;
using Desafio.Data.Interfaces;
using Desafio.Data.UnitOfWork;
using Desafio.Jobs.Tasks;
using Desafio.Services;
using Desafio.Services.Infra;
using Desafio.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Timers;
using VirusTotalServices;
using VirusTotalServices.Infra;
using VirusTotalServices.Interface;

namespace Desafio.Jobs
{
    class Program
    {
        static IConfiguration Configuration { get; set; }
        static IServiceProvider Provider { get; set; }

        private static ReportTask reportTask;
        private static Timer timer;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Provider = serviceCollection.BuildServiceProvider();

            reportTask = new ReportTask(Provider);
            reportTask.StartAsync();

            timer = new Timer();
            timer.Elapsed += OnTimedEvent;
            timer.Interval = 1000 * 60;
            timer.Enabled = true;

            Console.WriteLine("Press ENTER to leave");
            Console.ReadLine();
        }

        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            try
            {
                if ((DateTime.Now - reportTask.LastUpdate).TotalSeconds > 60)
                {
                    reportTask.UpdateAllReports();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.ToString()}");
            }
            finally
            {
                timer.Enabled = true;
            }
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<DesafioContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Transient);

            services.AddOptions();
            services.Configure<VirusTotalConfiguration>(Configuration.GetSection("VirusTotal"));
            services.Configure<QueueConfiguration>(Configuration.GetSection("Queue"));
            services.AddSingleton(Configuration);

            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new ReportMappingProfile());
            });

            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddTransient<IVirusTotalService, VirusTotalService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IEnqueueService, EnqueueService>();
        }
    }
}
