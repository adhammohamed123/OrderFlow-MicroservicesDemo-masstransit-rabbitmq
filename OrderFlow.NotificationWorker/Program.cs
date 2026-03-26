using MassTransit;
using OrderFlow.Infrastracture;
using OrderFlow.NotificationWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure();

builder.Services.AddMassTransit(buscfg =>
{
    //buscfg.AddConsumer<OrderCreatedConsumer>();
    buscfg.AddConsumers(typeof(Program).Assembly);
    buscfg.AddEntityFrameworkOutbox<ApplicationDbContext>(cfg =>
    {
        cfg.UseSqlServer();
        cfg.DuplicateDetectionWindow = TimeSpan.FromMinutes(30);
        cfg.QueryDelay = TimeSpan.FromSeconds(10);
    });
    buscfg.UsingRabbitMq((buscontext, rabbitbusfactorycfgtor) =>
    {
        rabbitbusfactorycfgtor.Host("localhost", "masstransit", hostcfg =>
        {
            hostcfg.Username("test");
            hostcfg.Password("123");

        });

        rabbitbusfactorycfgtor.ReceiveEndpoint("notification-service", endpoint =>
        {
            endpoint.UseEntityFrameworkOutbox<ApplicationDbContext>(buscontext);
            endpoint.ConfigureConsumer<PaymentProcessedConsumer>(buscontext);
             // we need to configure message ttl at queue level
             endpoint.SetQueueArgument("x-message-ttl",TimeSpan.FromMinutes(1));
             // we need to configure dead letter --> masstransit not create it automatically we need to create it manually and use it here for notification-service queue argument
             // also we can limit queue length (save send tokens) x-max-length,

            // also we can make this consumer work with batches (save object intialization of smtp)


            // add stock reserved consumer
            endpoint.ConfigureConsumer<StockReservedConsumer>(buscontext);
        });

        rabbitbusfactorycfgtor.ConfigureEndpoints(buscontext);
    });
});

var host = builder.Build();
host.Run();
