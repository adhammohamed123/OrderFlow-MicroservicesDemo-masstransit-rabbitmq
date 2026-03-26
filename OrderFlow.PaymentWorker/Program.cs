using MassTransit;
using OrderFlow.Contracts.Commands;
using OrderFlow.Contracts.MessagingInfra;
using OrderFlow.Infrastracture;
using Payment;

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
        //rabbitbusfactorycfgtor.Lazy= true;
        rabbitbusfactorycfgtor.ReceiveEndpoint("payment-service", endpoint => 
        {
            endpoint.UseEntityFrameworkOutbox<ApplicationDbContext>(buscontext);

            endpoint.ConfigureConsumer<OrderCreatedConsumer>(buscontext);
            //endpoint.Lazy= true;
        });
        EndpointConvention.Map<ReserveStock>(MassMap.InventoryEndpoint);
        rabbitbusfactorycfgtor.ConfigureEndpoints(buscontext);
    });
});

var host = builder.Build();
host.Run();
