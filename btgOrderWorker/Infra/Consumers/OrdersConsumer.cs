// Consumers/MyQueueConsumer.cs
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using btgOrderWorker.Domain.models;
using btgOrderWorker.Domain.interfaces;
using btgOrderWorker.Config;

namespace btgOrderWorker.Consumers
{
    public class OrdersConsumer : IOrdersConsumer
    {
        private readonly ILogger<OrdersConsumer> _logger;
        private readonly IOrderService _orderService;
        private readonly RabbitMQSettings _settings;
        private IConnection _connection;
        private IModel _channel;

        public OrdersConsumer
        (            ILogger<OrdersConsumer> logger,
            IOrderService orderService,
            IOptions<RabbitMQSettings> settings)
        {
            _logger = logger;
            _orderService = orderService;
            _settings = settings.Value;

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost,
                Port = _settings.Port
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public async Task StartConsumingAsync(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Received message: {Message}", message);

                try
                {
                    var myMessage = JsonSerializer.Deserialize<Order>(message);
                    if (myMessage != null)
                    {
                        await _orderService.ProcessMessageAsync(myMessage);
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message. Message will be sent to DLQ.");
                    _channel.BasicReject(ea.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(
                queue: _settings.QueueName,
                autoAck: false,
                consumer: consumer);

            _logger.LogInformation("Consumer started. Listening to queue: {Queue}", _settings.QueueName);

            await Task.Delay(-1, cancellationToken); 
        }
    }
}
