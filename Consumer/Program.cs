using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();

factory.Uri = new Uri("your connection string");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

string queName = "first-que";

//channel.QueueDeclare(queName, false, false, false);

var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(queName, true, consumer);

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Console.WriteLine(message);
};

Console.ReadKey();

