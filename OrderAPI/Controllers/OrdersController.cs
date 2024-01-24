using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.Models.Entities;
using OrderAPI.Models.Enums;
using OrderAPI.ViewModels;
using Shared.Events;
using Shared.Messages;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly OrderAPIDbContext _context;
        readonly IPublishEndpoint _publishEndpoint;
        public OrdersController(OrderAPIDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreatOrderVM creatOrder)
        {
            Order order = new()
            {
                OrderID = Guid.NewGuid(),
                BuyerID = creatOrder.BuyerId,
                CreatedDate = DateTime.Now,
                Statu = OrderStatus.Suspend
            };
            order.OrderItems = creatOrder.OrderItemList.Select(oi => new OrderItem
            {
                Count = oi.Count,
                Price = oi.Price,
                ProductId = oi.ProductId
            }).ToList();
            order.TotalPrice = creatOrder.OrderItemList.Sum(oi => oi.Price * oi.Count);
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            OrderCreatedEvent orderCreatedEvent = new()
            {
                BuyerId = order.OrderID,
                OrderId = order.OrderID,
                OrderItems = order.OrderItems.Select(oi => new OrderItemMessage
                {
                    Count = oi.Count,
                    ProductId = oi.ProductId
                }).ToList(),
                TotalPrice = order.TotalPrice
            };

            await _publishEndpoint.Publish(orderCreatedEvent);

            return Ok();
        }
    }
}
