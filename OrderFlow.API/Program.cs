using MassTransit;
using OrderFlow.Contracts.Commands;
using OrderFlow.Contracts.Events;
using OrderFlow.Contracts.Events.Order;
using OrderFlow.Infrastracture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();
builder.Services.AddMassTransit(buscfg =>
{
    //buscfg.SetKebabCaseEndpointNameFormatter(); 
    buscfg.AddEntityFrameworkOutbox<ApplicationDbContext>(cfg =>
    {
        cfg.UseSqlServer();
        cfg.UseBusOutbox();// api layer
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
        //rabbitbusfactorycfgtor.SendTopology.UseCorrelationId<OrderCreated>(o => o.Id);
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
