using MassTransit;
using OrderFlow.Contracts.Events;

namespace OrderFlow.PaymentWorker
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        private readonly ILogger<OrderCreatedConsumer> logger;

        public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
        {
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            try
            {

                #region firest smoke test
                //logger.LogInformation("Payment Service: Recieved A New Order: {OrderId} for Customer: {CustomerId} with a total Amount {Total}", context.Message.Id, context.Message.CustomerId, context.Message.TotalAmount);

                //await Task.Delay(2000);

                // logger.LogInformation("Payment Completed Successfuly for order {Order}", context.Message.Id);

                #endregion
                #region Add and Read message headers
                //read message headers
                //var messageid = context.MessageId;
                //var correlationid = context.CorrelationId;

                //Console.WriteLine(messageid + "----" + correlationid);

                //// read custom headers

                //var periority = context.Headers.Get<string>("periority");
                //var tanant = context.Headers.Get<string>("tanant-id");
                //Console.WriteLine(tanant + "----" + periority);

                #endregion

                Console.WriteLine($"Order Created: {context.Message.Id}--{context.Message.TotalAmount:C}");
                await Task.Delay(1000);

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Payment Failed For Order {Id}", context.Message.Id);
                throw;
            }
        }


        public class OrderGeneralEventConsumer : IConsumer<OrderEvent>
        {
            public async Task Consume(ConsumeContext<OrderEvent> context)
            {
                Console.WriteLine($"Order Event : {context.Message.Id}--- {context.Message.CreatedAtUtc}");
                await Task.Delay(1000);
            }
        }

        public class OrderCancelledConsumer : IConsumer<OrderCancelled>
        {
            public async Task Consume(ConsumeContext<OrderCancelled> context)
            {
                Console.WriteLine($"Order Cancelled: {context.Message.Id}--{context.Message.Reason}");
                await Task.Delay(1000);
            }
        }


    }
}
