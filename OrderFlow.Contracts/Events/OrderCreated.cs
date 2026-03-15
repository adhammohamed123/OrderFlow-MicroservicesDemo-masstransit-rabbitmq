using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Contracts.Events
{
    //public record OrderCreated(Guid Id,Guid CustomerId,decimal TotalAmount)
    //{
    //    public DateTime CreatedAtUtc { get; init; }= DateTime.Now;
    //}

    ///////////// mass transit use message initialization and polymorphisem
    public interface OrderEvent
    {
        public Guid Id { get; }
        public DateTime CreatedAtUtc { get; } 
    }

    public interface OrderCreated:OrderEvent
    {
        public Guid CustomerId { get;}
        public decimal TotalAmount { get; }

    }

    public interface OrderCancelled : OrderEvent
    {
        public string Reason { get; }
    }
}
