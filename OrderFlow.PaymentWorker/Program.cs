using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderFlow.Contracts.Commands;
using OrderFlow.Contracts.MessagingInfra;
using OrderFlow.Infrastracture;
using Payment;
using System.Data;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure();

builder.Services.AddMassTransit(buscfg =>
{
    //buscfg.AddConsumer<OrderCreatedConsumer>();
    buscfg.AddConsumers(typeof(Program).Assembly);
    //buscfg.AddEntityFrameworkOutbox<ApplicationDbContext>(cfg =>
    //{
    //    cfg.UseSqlServer();
    //    cfg.DuplicateDetectionWindow = TimeSpan.FromMinutes(30);
    //    cfg.QueryDelay = TimeSpan.FromSeconds(10);
    //});
    buscfg.AddInMemoryInboxOutbox();
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
            // endpoint.UseEntityFrameworkOutbox<ApplicationDbContext>(buscontext);

            // level 1 - in memory retry
            endpoint.UseMessageRetry(retrycfg => 
            {
                // Policy
                retrycfg.Immediate(3); 

                // handle
                retrycfg.Handle<DbUpdateConcurrencyException>();
                retrycfg.Handle<TimeoutException>();
                retrycfg.Handle<HttpRequestException>(
                    filter=> filter.StatusCode == System.Net.HttpStatusCode.GatewayTimeout ||
                    filter.StatusCode== System.Net.HttpStatusCode.ServiceUnavailable ||
                    filter.StatusCode== System.Net.HttpStatusCode.TooManyRequests);

                // ignore
                retrycfg.Ignore<InvalidOperationException>();
                retrycfg.Ignore<ApplicationException>();
                retrycfg.Ignore<NullReferenceException>();
                retrycfg.Ignore<HttpRequestException>(filter=>filter.StatusCode== System.Net.HttpStatusCode.Unauthorized);
            });

            endpoint.UseInMemoryInboxOutbox(buscontext);
            endpoint.ConfigureConsumer<OrderCreatedConsumer>(buscontext);
            //endpoint.Lazy= true;
        });
        EndpointConvention.Map<ReserveStock>(MassMap.InventoryEndpoint);
        rabbitbusfactorycfgtor.ConfigureEndpoints(buscontext);
    });
});

var host = builder.Build();
host.Run();
