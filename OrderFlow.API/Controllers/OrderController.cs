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

            await _publish.Publish(order,cancellationToken);

            return Accepted();
        }
    }
}
