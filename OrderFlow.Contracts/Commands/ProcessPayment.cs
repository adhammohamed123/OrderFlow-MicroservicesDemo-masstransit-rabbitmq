using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Contracts.Commands
{
    // send to payment service only
    public record ProcessPayment 
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; init; }
        public decimal Amount { get; init; }
        public string Currency { get; init; }="EGP";

        public string PaymentMethod { get; init; }=string.Empty;
        public string CardToken { get; init; } =string.Empty;    

    }
}
