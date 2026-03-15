namespace OrderFlow.Contracts.Events.Order
{
    // when customer receive order
    public record OrderCompleted 
    {
        public Guid OrderId { get; init; }
        public Guid CustomerId { get; init; }
        public DateTime CompletedAt { get; init; }
        public decimal FinalAmount { get; init; }
    }

}
