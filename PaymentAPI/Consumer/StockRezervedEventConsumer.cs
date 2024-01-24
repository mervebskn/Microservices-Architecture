using MassTransit;
using MassTransit.Transports;
using Shared.Events;

namespace PaymentAPI.Consumer
{
    public class StockRezervedEventConsumer : IConsumer<StockReservedEvents>
    {
        readonly IPublishEndpoint _publichEndpoint;
        public StockRezervedEventConsumer(IPublishEndpoint publichEndpoint)
        {

            _publichEndpoint = publichEndpoint;

        }
        public Task Consume(ConsumeContext<StockReservedEvents> context)
        {
            if (true)
            {
                //payment operations..
                PaymentCompletedEvent paymentCompletedEvent = new() { OrderId = context.Message.OrderId };
                _publichEndpoint.Publish(paymentCompletedEvent);
            }
            else
            {
                //payment failed..
            }
            return Task.CompletedTask;
        }
    }
}
