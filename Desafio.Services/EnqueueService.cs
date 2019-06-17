using Desafio.Services.Infra;
using Desafio.Services.Interfaces;
using Desafio.Services.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desafio.Services
{
    public class EnqueueService : IEnqueueService
    {
        private readonly QueueConfiguration configuration;

        public EnqueueService(IOptions<QueueConfiguration> configuration)
        {
            this.configuration = configuration.Value;
        }

        public void Enqueue(QueueMessage messageInput)
        {
            var factory = new ConnectionFactory() { HostName = configuration.HostName };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "desafio",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                    var message = JsonConvert.SerializeObject(messageInput);
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;

                    channel.BasicPublish(exchange: "",
                                 routingKey: "desafio",
                                 basicProperties: properties,
                                 body: body);
                }
            }
        } 
    }
}
