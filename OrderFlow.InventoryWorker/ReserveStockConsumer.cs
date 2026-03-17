using MassTransit;
using OrderFlow.Contracts.Commands;

namespace OrderFlow.InventoryWorker
{
    public class ReserveStockConsumer : IConsumer<ReserveStock>
    {
        public async Task Consume(ConsumeContext<ReserveStock> context)
        {
            Console.WriteLine($"We now Reserve Stock for Order {context.Message.OrderId}");
            await Task.Delay(3000);
            Console.WriteLine("Stock Reserved Successfully for Order"+context.Message.OrderId);
        }
    }
}
