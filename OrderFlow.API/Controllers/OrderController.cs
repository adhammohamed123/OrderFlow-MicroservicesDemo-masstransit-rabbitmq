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
          

            return Accepted();
        }
    }
}
