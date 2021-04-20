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
            Console.WriteLine("=====================================");
            Console.WriteLine("Welcome to app Costumers: ");


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
            var order = 0;
            Configure();

            

            while (order != 6)
            {
                Console.WriteLine("=====================================");
                Console.WriteLine("================MENU=================");
                Console.WriteLine("=====================================");
                Console.WriteLine("0--------> DONUTS--------------------");
                Console.WriteLine("1--------> BURGER--------------------");
                Console.WriteLine("2--------> SANDWICH------------------");
                Console.WriteLine("3--------> MEXICAN TACOS-------------");
                Console.WriteLine("4--------> CHICKEN NUGGETS-----------");
                Console.WriteLine("5--------> EXIT MENU-----------------");
                Console.WriteLine("=====================================");

                Console.WriteLine("SELECT YOUR ORDER PLEASE: ");
                order = int.Parse(Console.ReadLine());
                
                if (order!=5)
                {
                    var type = (Orderstypes.ORDER)order;

                    Console.WriteLine($"YOUR {type.ToString()} ORDER HAS BEEN SHIPPED!");

                    await _publisher.Publish(type, topic);

                    Thread.Sleep(1000);
                    Console.Clear();
                }
                

            } 
           

        }
    }
}
