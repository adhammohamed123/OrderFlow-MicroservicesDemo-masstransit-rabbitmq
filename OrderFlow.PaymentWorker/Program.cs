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

            endpoint.UseDelayedRedelivery(redeliverycfg =>
            {
                // redelivery for long time minites , hours ,days and for this is not in memory 
                redeliverycfg.Intervals(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(2));
            });
            // level 1 - in memory retry
            endpoint.UseMessageRetry(retrycfg => 
            {
                // Policy
                //retrycfg.Immediate(3); 
                //retrycfg.Interval(3,TimeSpan.FromSeconds(2)); // retry 3 times and between each retry 2s 
                //retrycfg.Intervals(
                //    TimeSpan.FromMilliseconds(500),
                //    TimeSpan.FromSeconds(1),
                //    TimeSpan.FromSeconds(3),
                //    TimeSpan.FromSeconds(5),
                //    TimeSpan.FromSeconds(7)
                //    );
                //retrycfg.Incremental(5,initialInterval:TimeSpan.FromSeconds(1),intervalIncrement:TimeSpan.FromSeconds(5));
                retrycfg.Exponential(5,minInterval: TimeSpan.FromMilliseconds(500),maxInterval: TimeSpan.FromSeconds(30),intervalDelta:TimeSpan.FromSeconds(2));
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
