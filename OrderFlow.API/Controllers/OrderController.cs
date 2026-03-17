using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Contracts.Dtos;
using OrderFlow.Contracts.Events;
using OrderFlow.Contracts.Events.Order;

namespace OrderFlow.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController(IPublishEndpoint publish) : ControllerBase
    {
        private readonly IPublishEndpoint _publish = publish;

        [HttpPost]
        public async Task<IActionResult> CreateNewOrder([FromQuery] string custname,[FromQuery] string custemail,CancellationToken cancellationToken)
        {
            var orderItems = new List<OrderItemDto>()
            {
                    new() { ProductId = NewId.NextSequentialGuid(), ProductName = "P1", Quantity = 2, UnitPrice = 10 },
                    new() { ProductId = NewId.NextSequentialGuid(), ProductName = "P2", Quantity = 1, UnitPrice = 50 },
            };


            var ordercreated = new OrderCreated
            {
                OrderId=NewId.NextSequentialGuid(),
                CustomerId= NewId.NextSequentialGuid(),
                CustomerName=custname,
                CustomerEmail=custemail,
                CreatedAt=InVar.Timestamp,
                OrderItems = orderItems,
                TotalAmount=orderItems.Sum(i=>i.Quantity*i.UnitPrice)
                
            };

            await _publish.Publish(ordercreated,context=>context.CorrelationId=ordercreated.OrderId,cancellationToken);


            return Accepted();
        }
    }
}
