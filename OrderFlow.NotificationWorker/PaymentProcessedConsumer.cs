using MassTransit;
using OrderFlow.Contracts.Events.Payment;

namespace OrderFlow.NotificationWorker
{
    public class PaymentProcessedConsumer : IConsumer<PaymentProcessed>
    {
        public async Task Consume(ConsumeContext<PaymentProcessed> context)
        {
            Console.WriteLine($"We Now Sending Notification to {context.Message.CustomerEmail}");
            Console.WriteLine($"TO: {context.Message.CustomerEmail}");
            Console.WriteLine("==============Payment Processed Successfully=================");
            Console.WriteLine($"Dear {context.Message.CustomerName}");
            Console.WriteLine($"we notify you for that payment processed sucessfully for order {context.Message.OrderId}");
            Console.WriteLine($"we received :{context.Message.TotalAmount} {context.Message.Currency}");
            await Task.CompletedTask;
        }
    }
}
