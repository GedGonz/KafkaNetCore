using Confluent.Kafka;
using Core.Kafka;
using Core.tools;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orders
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource source = new CancellationTokenSource();

            Console.WriteLine("==========================");
            Console.WriteLine("WELCOME TO ADMIN ORDERS");
            Console.WriteLine("LIST ORDERS");
            Console.WriteLine("==========================");

            ListeningTopic(source.Token);
            Console.ReadLine();
        }

        private static IConfigurationRoot Configure()
        {
            var config = new ConfigurationBuilder()
                         .AddJsonFile("appsettings.json")
                         .Build();

            return config;
        }

        private static Task[] ListeningTopic( CancellationToken token) 
        {
            var config = Configure();

            var _topic =config.GetSection("topic").Value;

            var _consumer = new Consumer<string, string>(config.GetToption<KafkaOprtions>("kafka"));


            _consumer.OnMessageConsumed += MessageConsumed;

            Task[] consumers = {
                _consumer.Consume(_topic,token)
            };

            return consumers;
        }

        private static Task MessageConsumed(Message<string, string> message) {

            var task  = Task.Factory.StartNew(() =>
            {
                var Order = (Orderstypes.ORDER)(int.Parse(message.Value));

                Console.WriteLine($"YOUR {Order.ToString()} ORDER HAS BEEN RECEIVED");

            });

            return task;
            
        }
    }
}
