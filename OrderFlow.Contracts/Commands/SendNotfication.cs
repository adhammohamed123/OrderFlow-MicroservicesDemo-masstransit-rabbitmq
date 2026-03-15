namespace OrderFlow.Contracts.Commands
{
    public record SendNotfication 
    {
        public Guid OrderId { get; init; }
        public string RecipentEmail { get; init; }=string.Empty;
        public string RecipentName { get; init; }=string.Empty;
        public string Subject { get; init; } = string.Empty;
        public string Body { get; init; } = string.Empty;
        public string NotficationType { get; init; } = "Email"; // email , sms , push
    }
}
