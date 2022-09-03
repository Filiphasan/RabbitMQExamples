using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using WatermarkMQ.Interfaces;

namespace WatermarkMQ.Implemantations
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        public static string ExchangeName = "ImageDirectExchange";
        public static string RoutingWatermark = "watermark-route-image";
        public static string QueueName = "queue-watermark-image";

        private readonly ILogger<RabbitMQService> _logger;

        public RabbitMQService(ConnectionFactory connectionFactory, ILogger<RabbitMQService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public Task SendToQueue<TEvent>(TEvent eventObj) where TEvent : class, IEventModel
        {
            var channel = ConnectRabbitMQ();
            var bodyString = JsonSerializer.Serialize(eventObj);

            var bodyByte = Encoding.UTF8.GetBytes(bodyString);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingWatermark, basicProperties: properties, body: bodyByte);
            return Task.CompletedTask;
        }

        private IModel ConnectRabbitMQ()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channel.IsOpen)
            {
                return _channel;
            }

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(ExchangeName, "direct", true, false);
            _channel.QueueDeclare(QueueName, true, false, false, null);
            _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingWatermark);

            _logger.LogInformation("RabbitMQ ile bağlantı kuruldu...");

            return _channel;
        }

        public ValueTask DisposeAsync()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbitMQ ile bağlantı koptu...");
            return new ValueTask();
        }
    }
}
