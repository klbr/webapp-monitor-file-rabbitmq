using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataDesafio;
using Desafio.Data.Interfaces;
using Desafio.Data.UnitOfWork;
using Desafio.Services;
using Desafio.Services.Infra;
using Desafio.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using VirusTotalServices;
using VirusTotalServices.Infra;
using VirusTotalServices.Interface;

namespace WebAppDesafio
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<DesafioContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            AddMapper(services);
            AddConfiguration(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(jo =>
            {
                jo.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };
            });

            AddServices(services);
        }

        private void AddConfiguration(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<VirusTotalConfiguration>(Configuration.GetSection("VirusTotal"));
            services.Configure<QueueConfiguration>(Configuration.GetSection("Queue"));
            services.AddSingleton(Configuration);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IVirusTotalService, VirusTotalService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IEnqueueService, EnqueueService>();
        }

        private static void AddMapper(IServiceCollection services)
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new ReportMappingProfile());
            });

            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
