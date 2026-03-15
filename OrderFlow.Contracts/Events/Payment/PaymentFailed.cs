namespace OrderFlow.Contracts.Events.Payment
{
    // if we try to process payment and end up with faild try
    public record PaymentFailed 
    {
        public Guid OrderId { get; init; }
        public Guid PaymentId { get; init; }
        public decimal Amount { get; init; }
        public string Currency { get; init; } = "EGP";

        public string Reason { get; init; } = string.Empty;
        public string ErrorCode { get; init; } = string.Empty;
        public DateTime FailedAt { get; init; }
    }

}
