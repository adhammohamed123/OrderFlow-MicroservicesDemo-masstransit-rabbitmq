using MassTransit;
using OrderFlow.Contracts.Commands;
using OrderFlow.Contracts.Events.Order;
using OrderFlow.Contracts.Events.Payment;
namespace Payment;
public class OrderCreatedConsumer : IConsumer<OrderCreated>
{

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        Console.WriteLine($"Payment Started to Process Order {context.Message.OrderId} -- {context.Message.CustomerName}-- {context.Message.TotalAmount} {context.Message.Currency}");
        await Task.Delay(2000);
        Console.WriteLine("Payment Done Successfully");

        var paymentProcessed = new PaymentProcessed()
        {
            Currency=context.Message.Currency,
            OrderId=context.Message.OrderId,
            CustomerEmail=context.Message.CustomerEmail,
            CustomerName=context.Message.CustomerName,
            PaymentId= Guid.NewGuid(),
            PaymentMethod="Card",
            ProcessedAt=InVar.Timestamp,
            TotalAmount=context.Message.TotalAmount
        };

       await context.Publish(paymentProcessed);

        var stock = new ReserveStock()
        {
            OrderId=context.Message.OrderId,
            OrderItems=context.Message.OrderItems
        };

        var endpoint = await context.GetSendEndpoint(new Uri("queue:inventory-service"));
        await endpoint.Send(stock);
       // await context.Publish(stock);
    }
}