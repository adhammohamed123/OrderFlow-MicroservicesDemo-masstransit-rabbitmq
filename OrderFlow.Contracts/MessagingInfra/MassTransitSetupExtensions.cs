using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlow.Contracts.MessagingInfra
{
    public class MassMap
    {
        #region Endpoints
        public static Uri NotificationEndpoint { get; } = new("queue:notification-service");
        //new(exchange:exchangeName)
        //new(topic:topicName)
        public static Uri InventoryEndpoint { get; } = new("queue:inventory-service");

        public static Uri PaymentEndpoint { get; } = new("queue:payment-service");

        #endregion


    }
}
