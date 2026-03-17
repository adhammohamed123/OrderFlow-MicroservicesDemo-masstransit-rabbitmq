using MassTransit;
using OrderFlow.Contracts.Commands;
using OrderFlow.Contracts.Events.Inventory;

namespace OrderFlow.InventoryWorker
{
    public class ReserveStockConsumer : IConsumer<ReserveStock>
    {
        public async Task Consume(ConsumeContext<ReserveStock> context)
        {
            Console.WriteLine($"We now Reserve Stock for Order {context.Message.OrderId}");
            await Task.Delay(3000);
            Console.WriteLine("Stock Reserved Successfully for Order"+context.Message.OrderId);

            var stockReserved = new StockReserved
            {
                OrderId = context.Message.OrderId,
                ReservationId = Guid.NewGuid(),
                ReservedAt = InVar.Timestamp,
                CustomerName =context.Message.CustomerName,
                CustomerEmail=context.Message.CustomerEmail,
                ReservatedItems = context.Message.OrderItems.Select(x => new ReservatedItemDto()
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    RemainingInStock = 10 - x.Quantity
                }).ToList()
            };
           await context.Publish(stockReserved);
        }
    }
}
