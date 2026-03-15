using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Contracts.Events
{
    public record OrderCreated(Guid Id,Guid CustomerId,decimal TotalAmount)
    {
        public DateTime CreatedAtUtc { get; init; }= DateTime.Now;
    }
   
}
