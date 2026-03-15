using OrderFlow.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Contracts.Events.Order
{
    // when order created 
    // payment service --> process payment
    // inventory service --> reserve stock
    // notfication service --> send notification to customer
    public record OrderCreated 
    {
        public Guid OrderId { get; init; }
        public Guid CustomerId { get; init; }
        public string CustomerName { get; init; }=string.Empty;
        public string CustomerEmail { get; init; }= string.Empty;

        public decimal TotalAmount { get; init; }
        public string Currency { get; init; } = "EGP";
        public List<OrderItemDto> OrderItems { get; init; } = [];
        public AddressDto ShippingAddress { get; init; } = new();
        public DateTime CreatedAt { get; init; }
    }

}
