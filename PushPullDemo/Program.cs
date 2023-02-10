// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
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

void ReadMessageWithPushModel()
{
    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (sender, e) =>
    {
        string message = Encoding.UTF8.GetString(e.Body.ToArray());
        Console.WriteLine("Message :" + message);
    };

    string consumerTag = channel.BasicConsume("my.queue1", true, consumer);
    Console.WriteLine("Subscribed. press any key to unsubscribe and exit");
    Console.ReadKey();
    channel.BasicCancel(consumerTag);
}

void ReadMessageWithPullModel()
{
    Console.WriteLine("Reading message from queue. press 'e' to exit");
    while (true)
    {
        Console.WriteLine("Trying to get a message from the queue");
        var result = channel.BasicGet("my.queue1", true);
        if (result !=null)
        {
            var message = Encoding.UTF8.GetString(result.Body.ToArray());
        }
        if (Console.KeyAvailable)
        {
            var keyinfo = Console.ReadKey();
            if (keyinfo.KeyChar=='e' || keyinfo.KeyChar == 'E')
            {
                return;
            }
          
        }
        Thread.Sleep(2000);
    }
}