using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Models;
using OrderAPI.Models.Entities;
using OrderAPI.Models.Enums;
using Shared.Events;

namespace OrderAPI.Consumers
{
    public class PaymentComletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        readonly OrderAPIDbContext _orderAPIDbContext;

        public PaymentComletedEventConsumer(OrderAPIDbContext orderAPIDbContext)
        {
            _orderAPIDbContext = orderAPIDbContext;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            Order order = await _orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderID == context.Message.OrderId);
            order.Statu = OrderStatus.Completed;
        }
    }
}
