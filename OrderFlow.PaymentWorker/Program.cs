using MassTransit;
using OrderFlow.PaymentWorker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddMassTransit(buscfg =>
{
    buscfg.AddConsumer<OrderCreatedConsumer>();
    buscfg.UsingRabbitMq((buscontext, rabbitbusfactorycfgtor) =>
    {
        rabbitbusfactorycfgtor.Host("localhost", "masstransit", hostcfg =>
        {
            hostcfg.Username("test");
            hostcfg.Password("123");

        });

        rabbitbusfactorycfgtor.ConfigureEndpoints(buscontext);
    });
});

var host = builder.Build();
host.Run();
