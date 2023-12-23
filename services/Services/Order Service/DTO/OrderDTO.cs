namespace Services.Services.Order_Service.DTO
{
    public class OrderDTO
    {
        public string BasketId { get; set; }
        public string BuyerEmail { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDTO ShippingAddress { get; set; }

    }
}
