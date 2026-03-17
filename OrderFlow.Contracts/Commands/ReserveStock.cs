using OrderFlow.Contracts.Dtos;

namespace OrderFlow.Contracts.Commands
{
    public record ReserveStock 
    {
        public Guid OrderId { get; init; }
        public Guid CustomerId { get; init; }
        public string CustomerName { get; init; } = default!;
        public string CustomerEmail { get; init; } = default!;
        public List<OrderItemDto> OrderItems { get; init; } = [];
    }
}
