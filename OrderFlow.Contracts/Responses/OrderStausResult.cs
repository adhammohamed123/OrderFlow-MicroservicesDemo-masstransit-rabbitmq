namespace OrderFlow.Contracts.Responses
{
    public record OrderStausResult 
    {
        public Guid OrderId { get; init; }
        public string Status { get; init; }=string.Empty;
        public string PaymentStatus { get; init; }= string.Empty;
        public string ShippingStatus { get; init; }=string.Empty;

        public DateTime LastUpdated { get; init; }
    }
}
