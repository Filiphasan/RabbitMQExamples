using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory();

factory.Uri = new Uri("your connection string");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

string queName = "first-que";

channel.QueueDeclare(queName, false, false, false);

string queMessage = "Hello World!";

var queBytes = Encoding.UTF8.GetBytes(queMessage);

channel.BasicPublish(string.Empty, queName, null, queBytes);


Console.WriteLine("Mesaj Kuyruğa iletilmiştir.");

Console.ReadKey();

