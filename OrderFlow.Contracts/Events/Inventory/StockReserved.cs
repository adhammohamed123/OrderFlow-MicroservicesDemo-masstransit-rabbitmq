namespace OrderFlow.Contracts.Events.Inventory
{
    public record StockReserved 
    {
        public Guid OrderId { get; init; }
        public Guid ReservationId { get; init; }

        public List<ReservatedItemDto> ReservatedItems { get; init; } = [];
        public DateTime ReservedAt { get; init; }
    }
    public record ReservatedItemDto
    {
        public Guid ProductId { get; init; }
        public int Quantity { get; init; }
        public int RemainingInStock { get; init; }
    }

}
