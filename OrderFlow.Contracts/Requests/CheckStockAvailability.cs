using OrderFlow.Contracts.Dtos;

namespace OrderFlow.Contracts.Requests
{
    public record CheckStockAvailability 
    {
        public List<OrderItemDto> OrderItems { get; init; } = [];
    }
}
