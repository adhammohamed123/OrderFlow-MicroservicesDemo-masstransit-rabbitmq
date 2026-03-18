using MassTransit;
using OrderFlow.Contracts.Events.Inventory;
using OrderFlow.Contracts.Events.Payment;

namespace OrderFlow.NotificationWorker
{
    public class PaymentProcessedConsumer : IConsumer<Batch<PaymentProcessed>>
    {
        private static async Task Consume(ConsumeContext<PaymentProcessed> context)
        {
                if (context.Message.CustomerName == "adham")
                    throw new InvalidOperationException("Customer Blocked");

                Console.WriteLine($"We Now Sending Notification to {context.Message.CustomerEmail}");
                Console.WriteLine($"TO: {context.Message.CustomerEmail}");
                Console.WriteLine("==============Payment Processed Successfully=================");
                Console.WriteLine($"Dear {context.Message.CustomerName}");
                Console.WriteLine($"we notify you for that payment processed sucessfully for order {context.Message.OrderId}");
                Console.WriteLine($"we received :{context.Message.TotalAmount} {context.Message.Currency}");
                await Task.CompletedTask;
           
        }

        public async Task Consume(ConsumeContext<Batch<PaymentProcessed>> context)
        {
            foreach (var consumeContext in context.Message)
            {
                try
                {
                  await Consume(consumeContext);
                }
                catch (Exception)
                {
                    Console.WriteLine("Eror🙄occured!");
                    await context.Publish<Fault<PaymentProcessed>>(consumeContext.Message);
                }
            }
        }
    }

    public class PaymentProcessedConsumerDefinition : ConsumerDefinition<PaymentProcessedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<PaymentProcessedConsumer> consumerConfigurator, IRegistrationContext context)
        {
            consumerConfigurator.Options<BatchOptions>(options => options.SetConcurrencyLimit(2).SetMessageLimit(100).SetTimeLimit(TimeSpan.FromSeconds(10)));
        }
    }



    public class StockReservedConsumer : IConsumer<Batch<StockReserved>>
    {
        public async Task Consume(ConsumeContext<Batch<StockReserved>> context)
        {
            foreach (var consumecontext in context.Message)
            {
               await Consume(consumecontext);
            }
        }

        public static async Task Consume(ConsumeContext<StockReserved> context)
        {
            Console.WriteLine($"We Now Sending Notification to {context.Message.CustomerEmail}");
            Console.WriteLine($"TO: {context.Message.CustomerEmail}");
            Console.WriteLine("==============Stock Reserved Successfully=================");
            Console.WriteLine($"Dear {context.Message.CustomerName}");
            Console.WriteLine($"we notify you for that your Stock was reserved sucessfully {context.Message.ReservationId} for order {context.Message.OrderId}");
            Console.WriteLine($"we reserve for you :{context.Message.ReservatedItems.Count} item ");
            await Task.CompletedTask;
        }
    }

    public class StockReservedConsumerDefinition : ConsumerDefinition<StockReservedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<StockReservedConsumer> consumerConfigurator, IRegistrationContext context)
        {
            consumerConfigurator.Options<BatchOptions>(options=> options.SetConcurrencyLimit(5).SetMessageLimit(20).SetTimeLimit(TimeSpan.FromSeconds(15)));
        }
    }

}
