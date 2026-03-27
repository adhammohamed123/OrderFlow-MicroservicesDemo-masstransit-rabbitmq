using MassTransit;
using MassTransit.RabbitMqTransport.Configuration;
using Microsoft.EntityFrameworkCore;
using OrderFlow.Contracts.Commands;
using OrderFlow.Contracts.Events.Order;
using OrderFlow.Contracts.Events.Payment;
namespace Payment;
public class OrderCreatedConsumer : IConsumer<OrderCreated>
{

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        // simulate eror to retry
        Console.WriteLine($"Attempt: {context.GetRetryAttempt()} --Previous Retry Count: {context.GetRetryCount()}");
        if(context.Message.CustomerName=="DbDown")
        {
            throw new DbUpdateConcurrencyException("Database is down now we will retry again");

            //DbUpdateConcurrencyException()
            //HttpRequestException()
            //TimeoutException()
        }
        else if(context.Message.CustomerName=="Eror")
        {
            throw new InvalidOperationException("Eror Customer Is Blocked");
        }






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
            CustomerEmail= context.Message.CustomerEmail,
            CustomerName=context.Message.CustomerName,
            CustomerId=context.Message.CustomerId,
            OrderItems=context.Message.OrderItems
        };

        //var endpoint = await context.GetSendEndpoint(new Uri("queue:inventory-service"));
       // await endpoint.Send(stock);
       await context.Send(stock);
       // await context.Publish(stock);
    }
}

//public class OrderCreatedConsumerDefinition : ConsumerDefinition<OrderCreatedConsumer>
//{
//    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<OrderCreatedConsumer> consumerConfigurator, IRegistrationContext context)
//    {
//        endpointConfigurator.UseInMemoryOutbox(context);
//        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
//            //rmq.Lazy = true;
//    }
//}


public class OrderCreationFaultConsumer : IConsumer<Fault<OrderCreated>>
{
    public async Task Consume(ConsumeContext<Fault<OrderCreated>> context)
    {
        Console.WriteLine("Do some logic in case of order creation failed and all retries and redliveries are consumed");
        OrderCreated order =  context.Message.Message;
        Console.WriteLine("BackWord Logic for order " +order.OrderId);
         await Task.CompletedTask;
    }
}