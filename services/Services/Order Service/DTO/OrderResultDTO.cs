using Core.Entities.Order_Entities;

namespace Services.Services.Order_Service.DTO
{
    public class OrderResultDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public AddressDTO ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public IReadOnlyList<OrderItemDTO> OrderItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Total { get; set; }
        public string? PaymentIntentId { get; set; }

    }
}
