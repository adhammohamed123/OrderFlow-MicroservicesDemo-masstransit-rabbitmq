using MassTransit;
using OrderFlow.Infrastracture;
using OrderFlow.InventoryWorker;

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

        rabbitbusfactorycfgtor.ReceiveEndpoint("inventory-service", endpoint =>
        {
            endpoint.UseEntityFrameworkOutbox<ApplicationDbContext>(buscontext);
            endpoint.ConfigureConsumer<ReserveStockConsumer>(buscontext);
        });

        rabbitbusfactorycfgtor.ConfigureEndpoints(buscontext);
    });
});

var host = builder.Build();
host.Run();
