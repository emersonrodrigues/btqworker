using btgOrderWorker.Config;
using btgOrderWorker.Consumers;
using btgOrderWorker.Domain.interfaces;
using btgOrderWorker.infra.Repositories;
using btgOrderWorker.services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<IOrdersConsumer, OrdersConsumer>();
builder.Services.AddTransient<IOrderRepository,OrderRepository>();
builder.Services.AddTransient<IProductRepository,ProductRepository>();
builder.Services.AddTransient<IOrderService,OrderService>();
builder.Services.AddHostedService<OrderWorker>();
// builder.Configuration.GetValue<string>("connectionstring");

var host = builder.Build();
host.Run();
