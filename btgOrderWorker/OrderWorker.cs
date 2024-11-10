
using btgOrderWorker.Domain.interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


public class OrderWorker : BackgroundService
{
    private readonly ILogger<OrderWorker> _logger;
    private readonly IOrdersConsumer _ordersConsumer;

    public OrderWorker(ILogger<OrderWorker> logger, IOrdersConsumer ordersConsumer)
    {
        _logger = logger;
        _ordersConsumer = ordersConsumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
        await _ordersConsumer.StartConsumingAsync(stoppingToken);
    }
}

