using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Models;
using OrderAPI.Models.Entities;
using OrderAPI.Models.Enums;
using OrderAPI.ViewModels;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        readonly OrderAPIDbContext _context;
        public OrdersController(OrderAPIDbContext context)
        {
                _context= context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreatOrderVM creatOrder)
        {
            Order order = new()
            {
                OrderID=Guid.NewGuid(),
                BuyerID=creatOrder.BuyerId,
                CreatedDate=DateTime.Now,
                Statu=OrderStatus.Suspend
            };
            order.OrderItems = creatOrder.OrderItemList.Select(oi => new OrderItem
            {
                Count = oi.Count,
                Price = oi.Price,
                ProductId = oi.ProductId
            }).ToList();
            order.TotalPrice = creatOrder.OrderItemList.Sum(oi => oi.Price);
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
