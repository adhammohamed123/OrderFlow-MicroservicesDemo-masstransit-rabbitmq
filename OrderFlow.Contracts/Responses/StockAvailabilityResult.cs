namespace OrderFlow.Contracts.Responses
{
    public record StockAvailabilityResult 
    {
        public bool IsAvailable { get; init; }
        public List<StockCheckResult> StockCheckResults { get; init; } = [];
    }

    public record StockCheckResult
    {
        public Guid ProductId  { get; init; }
        public int  RequestedQuantity  { get; init; }
        public  int AvailableQuantity  { get; init; }

        public bool Issufficient => AvailableQuantity>=RequestedQuantity;
    }
}
