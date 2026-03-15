using OrderFlow.Contracts.Dtos;

namespace OrderFlow.Contracts.Commands
{
    public record ReserveStock 
    {
        public Guid OrderId { get; init; }
        public List<OrderItemDto> OrderItems { get; init; } = [];
    }
}
