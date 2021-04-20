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
            Console.WriteLine("Welcome to admin Orders: ");
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
            var _consumer = new Consumer<string, string>(config.GetToption<KafkaOprtions>("kafka"));

            _consumer.OnMessageConsumed += MessageConsumed;

            Task[] consumers = {
                _consumer.Consume("order",token)
            };

            return consumers;
        }

        private static Task MessageConsumed(Message<string, string> message) {

            var task  = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Enter order: {message.Value}");
            });

            return task;
            
        }
    }
}
