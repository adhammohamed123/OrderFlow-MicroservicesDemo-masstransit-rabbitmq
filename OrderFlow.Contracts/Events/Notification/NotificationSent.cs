namespace OrderFlow.Contracts.Events.Notification
{
    public record NotificationSent 
    {
        public Guid OrderId { get; init; }
        public Guid CustomerId { get; init; }
        public string CustomerName { get; init; } = string.Empty;
        public string CustomerEmail { get; init; }= string.Empty;
        public DateTime SentAt { get; init; }
    }

}
