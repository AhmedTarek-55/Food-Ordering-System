namespace Infrastructure.Basket_Repository.Basket_Entities
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public int? DeliveryMethodID { get; set; }
        public decimal ShippingPrice { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}