using Desafio.Services.Infra;
using Desafio.Services.Interfaces;
using Desafio.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Desafio.Jobs.Tasks
{
    public class ReportTask
    {
        private readonly QueueConfiguration configuration;
        private readonly IServiceProvider provider;
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;

        public DateTime LastUpdate { get; private set; } = DateTime.Now;

        public ReportTask(IServiceProvider provider)
        {
            this.configuration = provider.GetService<IOptions<QueueConfiguration>>().Value;
            this.provider = provider;
        }

        public async void StartAsync()
        {
            Initilize();

            while (true)
            {
                var delayTime = 15;
                Console.WriteLine($"{DateTime.Now} - Delay by {delayTime} seconds");
                await Task.Delay(1000 * delayTime);

                try
                {
                    var data = channel.BasicGet("desafio", true);
                    Received(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.ToString()}");
                    Initilize();
                }
            }
        }

        private void Initilize()
        {
            factory = new ConnectionFactory() { HostName = configuration.HostName };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        private void Received(BasicGetResult data)
        {
            if (data != null)
            {
                var body = data.Body;
                var message = Encoding.UTF8.GetString(body);
                ProcessQueue(message);
                LastUpdate = DateTime.Now;
            }
            Console.WriteLine($"{DateTime.Now} - Finished");
        }

        internal void UpdateAllReports()
        {
            var reportService = provider.GetService<IReportService>();
            reportService.UpdateAllReports();
            LastUpdate = DateTime.Now.AddDays(1);
        }

        private void ProcessQueue(string message)
        {
            var queueMessage = JsonConvert.DeserializeObject<QueueMessage>(message);

            Console.WriteLine($"{queueMessage.Type} Received: {queueMessage.FileName ?? queueMessage.Resource}");

            var reportService = provider.GetService<IReportService>();
            switch (queueMessage.Type)
            {
                case QueueType.CreateReport:
                    reportService.DequeueCreateReport(queueMessage.Id, queueMessage.FileName, queueMessage.Data);
                    break;

                default:
                    reportService.DequeueUpdateReport(queueMessage.Id, queueMessage.Resource);
                    break;
            }
        }
    }
}
