using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class RabbitMQSettings
    {
        public const string Stock_OrderCreatedEventQueue = "stock-order-created-event-q";
        public const string Payment_StockReservedEventQueue = "payment-stock-reserved-event-q";
        public const string Order_PaymentCompletedEventQueue = "order-payment-completed-event-q";

    }
}
