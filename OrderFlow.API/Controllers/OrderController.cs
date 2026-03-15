using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Contracts.Events;

namespace OrderFlow.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController(IPublishEndpoint publish) : ControllerBase
    {
        private readonly IPublishEndpoint _publish = publish;

        [HttpPost]
        public async Task<IActionResult> CreateNewOrder(CancellationToken cancellationToken)
        {
            var order = new OrderCreated(Id: Guid.NewGuid(), CustomerId: Guid.NewGuid(), 150);

            await _publish.Publish(order, context =>
            {
                context.Headers.Set("tanant-id", "tanant-123");
                context.Headers.Set("periority","high");
                context.Headers.Set("num", 120);
                context.Headers.Set("TTL", "10s");
                context.TimeToLive = TimeSpan.FromSeconds(10);
            },cancellationToken);

            return Accepted();
        }
    }
}
