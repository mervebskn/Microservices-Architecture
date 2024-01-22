namespace OrderAPI.ViewModels
{
    public class CreatOrderVM
    {
        public Guid BuyerId { get; set; }
        public List<CreateOrderItemVM> OrderItemList { get; set; }
    }
    public class CreateOrderItemVM
    {
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

    }
}
