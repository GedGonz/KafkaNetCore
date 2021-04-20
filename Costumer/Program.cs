using Core.Kafka;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Core.tools;
using System.Threading;

namespace Costumer
{
    class Program
    {

        private static Publisher _publisher;
        static async Task Main(string[] args)
        {
            Console.WriteLine("==========================");
            Console.WriteLine("Welcome to app Costumers: ");
            Console.WriteLine("==========================");


           await PublishOrder("order");
        }
        private static IConfigurationRoot Configure() 
        {
               var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

            _publisher = new Publisher(config.GetToption<KafkaOprtions>("kafka"));

            return config;
        }

        private static async Task PublishOrder(string topic)
        {
            var order = string.Empty;
            Configure();
            while(order != "q") {

                Console.WriteLine("Please digit to Order: ");
                order = Console.ReadLine();

                await _publisher.Publish(order, topic);

                Thread.Sleep(1000);
            }
           

        }
    }
}
