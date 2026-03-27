using MassTransit;
using OrderFlow.Infrastracture;
using Quartz;
using CrystalQuartz.AspNetCore; // تأكد من إضافة الـ using ده

var builder = WebApplication.CreateBuilder(args);

// 1. الإعدادات العادية
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();

// 2. إعداد MassTransit (كما هو لديك)
builder.Services.AddMassTransit(buscfg =>
{
    buscfg.AddEntityFrameworkOutbox<ApplicationDbContext>(cfg =>
    {
        cfg.UseSqlServer();
        cfg.UseBusOutbox();
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
    });
});

// 3. إعداد Quartz للاتصال بالداتابيز
builder.Services.AddQuartz(q =>
{
    q.SchedulerName = "Runner"; // لازم يطابق اسم الـ Scheduler في الـ Worker
    q.UsePersistentStore(p =>
    {
        var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Masstransit;Integrated Security=True;Trust Server Certificate=True;";
        p.UseSqlServer(connectionString);
        p.UseNewtonsoftJsonSerializer();
    });
});

var app = builder.Build();

// 4. إعداد الـ Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 5. تفعيل CrystalQuartz UI
// بنجيب الـ Scheduler من الـ DI ونمرره للـ UI
var schedulerFactory = app.Services.GetRequiredService<ISchedulerFactory>();
var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

// المسار الافتراضي هيكون /quartz
app.UseCrystalQuartz(() => scheduler);

app.MapControllers();

app.Run();