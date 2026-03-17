namespace OrderFlow.Contracts.Events.Payment
{
    // trigger when a payment processed of order successfuly
    public record PaymentProcessed 
    {
        public Guid OrderId { get; init; }
        public Guid PaymentId { get; init; }
        public string CustomerName { get; init; } = default!;
        public string CustomerEmail { get; init; }= default!;

        public decimal TotalAmount { get; init; }
        public string Currency { get; init; } = "EGP";
        public string PaymentMethod { get; init; }=string.Empty;
        public DateTime ProcessedAt { get; init; }
    }

}
