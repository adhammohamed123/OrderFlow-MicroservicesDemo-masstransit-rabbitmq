using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Contracts.Dtos
{
    public record OrderItemDto 
    {
        public Guid ProductId { get; init; }
        public string ProductName { get; init; } = string.Empty;

        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }

        public decimal SubTotal => Quantity*UnitPrice;

    }
}
