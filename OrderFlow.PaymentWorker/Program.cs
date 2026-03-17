using MassTransit;
using Payment;

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

        rabbitbusfactorycfgtor.ReceiveEndpoint("payment-service", endpoint =>
        {
            endpoint.ConfigureConsumer<OrderCreatedConsumer>(buscontext);
        });

        rabbitbusfactorycfgtor.ConfigureEndpoints(buscontext);
    });
});

var host = builder.Build();
host.Run();
