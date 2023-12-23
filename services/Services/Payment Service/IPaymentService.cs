using Services.Services.Basket_Service.DTO;
using Services.Services.Order_Service.DTO;

namespace Services.Services.Payment_Service
{
    public interface IPaymentService
    {
        Task<CustomerBasketDTO> CreateOrUpdatePaymentIntent(string basketId);
        Task<OrderResultDTO> UpdateOrderPaymentSucceeded(string PaymentIntentId, string basketId);
        Task<OrderResultDTO> UpdateOrderPaymentFailed(string PaymentIntentId);
    }
}