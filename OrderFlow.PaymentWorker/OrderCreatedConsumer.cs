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

                logger.LogInformation("Payment Service: Recieved A New Order: {OrderId} for Customer: {CustomerId} with a total Amount {Total}", context.Message.Id, context.Message.CustomerId, context.Message.TotalAmount);

                await Task.Delay(2000);

                 logger.LogInformation("Payment Completed Successfuly for order {Order}", context.Message.Id);
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Payment Failed For Order {Id}",context.Message.Id);
                throw;
            }
        }
    }

    
}
