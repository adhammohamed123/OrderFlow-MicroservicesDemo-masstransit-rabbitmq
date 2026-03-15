using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Contracts.Requests
{
    public record GetOrderStatus 
    {
        public Guid OrderId { get; init; }
    }
}
