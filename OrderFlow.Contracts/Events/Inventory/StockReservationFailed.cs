namespace OrderFlow.Contracts.Events.Inventory
{
    public record StockReservationFailed 
    {
        public Guid OrderId { get; init; }
        public DateTime FailedAt { get; init; }

        public List<InsuffitiontStockItemDto> InsuffitiontStockItems { get; init; } = [];
    }

    public record InsuffitiontStockItemDto
    {
        public Guid ProductId { get; init; }
        public string ProductName { get; init; } = string.Empty;

        public int RequestedQuantity { get; init; }
        public int AvailableQuantity { get; init; }

    }

}
