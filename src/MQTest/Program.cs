﻿using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MQTest;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "192.168.2.200",Port = 5672,UserName = "admin",Password = "visual2010"};
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "webhook.chat",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
        };
        channel.BasicConsume(queue: "webhook.chat",
            autoAck: true,
            consumer: consumer);

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}