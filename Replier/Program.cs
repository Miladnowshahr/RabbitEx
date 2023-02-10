// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Unicode;

Console.WriteLine("Hello, World!");

IConnection connection;
IModel channel;

var factory = new ConnectionFactory();
factory.HostName = "localhost";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";
factory.VirtualHost = "/";

connection = factory.CreateConnection();
channel = connection.CreateModel();


var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, e) =>
{
    string request = System.Text.Encoding.UTF8.GetString(e.Body.Span);

    Console.WriteLine("Request  received:" + request);

    string response = $"Response for {request}";
    channel.BasicPublish("", "responses", null, Encoding.UTF8.GetBytes(response ));
};
channel.BasicConsume("request", true, consumer);

Console.WriteLine("Press any to exit");
Console.ReadKey(); 

