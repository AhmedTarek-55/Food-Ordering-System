namespace Services.Services.Order_Service.DTO
{
    public class OrderItemDTO
    {
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string PictureURL { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
