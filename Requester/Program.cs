// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

IConnection connection;
IModel channel;

var factory = new ConnectionFactory();
factory.HostName= "localhost";
factory.Port = 5672;
factory.UserName = "guest";
factory.Password = "guest";
factory.VirtualHost = "/";

connection= factory.CreateConnection();
channel= connection.CreateModel();

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, e) =>
{
    string message = System.Text.Encoding.UTF8.GetString(e.Body.Span);

    Console.WriteLine("Response received:"+ message);
};

channel.BasicConsume("responses", true, consumer);

while (true)
{
    Console.WriteLine("Enter your request:");
    string request = Console.ReadLine();
    if (request =="exit")
    {
        break;
    }
    channel.BasicPublish("","request",null,Encoding.UTF8.GetBytes(request));
}

channel.Close();
connection.Close();