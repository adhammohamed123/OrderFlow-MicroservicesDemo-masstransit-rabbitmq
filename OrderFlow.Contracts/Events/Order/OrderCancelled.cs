namespace OrderFlow.Contracts.Events.Order
{
    // when order cancelled 
    // payment service --> refund
    // inventory service --> return stock
    // notification service --> notify customer
    public record OrderCancelled 
    {
        public Guid  OrderId { get; init; }
        public Guid CustomerId { get; init; }
        public Guid CancelledBy { get; init; }
        public string Resoan { get; init; } = string.Empty;
        public DateTime CancelledAt { get; init; }
    }

}
