using Quartz;
namespace Payment;

[DisallowConcurrentExecution] // ensure one instance running
public class SimpleTestQuartzClusterJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"Job Running at {DateTime.Now} on Instance: {context.Scheduler.SchedulerInstanceId}");
        await Task.CompletedTask;
    }
}