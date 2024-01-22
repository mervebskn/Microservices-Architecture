using OrderAPI.Models.Enums;

namespace OrderAPI.Models.Entities
{
    public class Order
    {
        public Guid OrderID { get; set; }
        public Guid BuyerID { get; set; } 
        public decimal TotalPrice { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public OrderStatus Statu { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
