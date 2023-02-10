// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, World!");

IConnection connection;

IModel channel;

var factory = new ConnectionFactory();
factory.UserName = "guest";
factory.Password = "guest";
factory.HostName = "localhost";
factory.Port = 5672;
factory.VirtualHost = "/";
connection = factory.CreateConnection();
channel = connection.CreateModel();

channel.ExchangeDeclare("ex.fanout", "fanout", true, false, null);

channel.ExchangeDeclare("ex.direct", "direct", true, false, new Dictionary<string, object>
{
    {"alterante-exchange","ex.fanout" }
});

channel.QueueDeclare("my.queue1", true, false, false, null);
channel.QueueDeclare("my.queue2", true, false, false, null);
channel.QueueDeclare("my.queue3", true, false, false, null);


channel.QueueBind("my.queue1", "ex.direct", "video");
channel.QueueBind("my.queue2", "ex.direct", "image");
channel.QueueBind("my.unroutedQueue", "ex.fanout", "");

channel.BasicPublish("ex.direct", "video", null,Encoding.UTF8.GetBytes("Message with video routing key"));
channel.BasicPublish("ex.direct", "image", null,Encoding.UTF8.GetBytes("Message with image routing key"));
channel.BasicPublish("ex.direct", "text", null,Encoding.UTF8.GetBytes("Message without any routing key"));