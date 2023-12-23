using Core.Entities;
using Services.Services.Order_Service.DTO;

namespace Services.Services.Order_Service
{
    public interface IOrderService
    {
        Task<OrderResultDTO> CreateOrderAsync(OrderDTO orderDto);
        Task<IReadOnlyList<OrderResultDTO>> GetAllOrdersForUserAsync(string buyerEmail);
        Task<OrderResultDTO> GetOrderByIdAsync(int Id, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync();
    }
}
