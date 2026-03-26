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

            // publish may take long time , we don't need user to wait a long (note here we publish directly to Broker and not use Outbox pattern)
            // so we need to support canclation of publish (using cancelation token + timer token )--> linked cancelation token 

            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            using var linkedCts= CancellationTokenSource.CreateLinkedTokenSource(cancellationToken,timeoutCts.Token);

            try
            {
                await _publish.Publish(ordercreated, context => context.CorrelationId = ordercreated.OrderId, linkedCts.Token);//cancellationToken);
            }
            catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
            {
                return StatusCode(StatusCodes.Status504GatewayTimeout, "Publish is slow and take long time try again later");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("user request cancelation for his request and no need to send response from us");
            }
            //catch (OperationCanceledException)
            //{
            //    if(timeoutCts.IsCancellationRequested)
            //        return StatusCode(StatusCodes.Status504GatewayTimeout, "Publish is slow and take long time try again later");

            //    Console.WriteLine("user request cancelation for his request and no need to send response from us");
            //}
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            // note that in real scenario we will use outbox pattern to save into DB in Same Transaction + ( outbox delivery worker in masstransit) --> publish to broker

            return Accepted();
        }
    }
}
