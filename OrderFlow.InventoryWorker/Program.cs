using MassTransit;
using OrderFlow.InventoryWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransit(buscfg =>
{
    //buscfg.AddConsumer<OrderCreatedConsumer>();
    buscfg.AddConsumers(typeof(Program).Assembly);
    buscfg.UsingRabbitMq((buscontext, rabbitbusfactorycfgtor) =>
    {
        rabbitbusfactorycfgtor.Host("localhost", "masstransit", hostcfg =>
        {
            hostcfg.Username("test");
            hostcfg.Password("123");

        });

        rabbitbusfactorycfgtor.ReceiveEndpoint("inventory-service", endpoint =>
        {
            endpoint.ConfigureConsumer<ReserveStockConsumer>(buscontext);
        });

        rabbitbusfactorycfgtor.ConfigureEndpoints(buscontext);
    });
});

var host = builder.Build();
host.Run();
