using AutoMapper;
using Core.Entities;
using Core.Entities.Order_Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications.Orders;
using Microsoft.Extensions.Configuration;
using Services.Services.Basket_Service;
using Services.Services.Basket_Service.DTO;
using Services.Services.Order_Service.DTO;
using Stripe;
using Product = Core.Entities.Product;

namespace Services.Services.Payment_Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketService _basketService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IBasketService basketService, IConfiguration configuration, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _basketService = basketService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<CustomerBasketDTO> CreateOrUpdatePaymentIntent(string basketId)
        {
            // 1. Set the secret key for the Stripe API
            StripeConfiguration.ApiKey = _configuration["Stripe:Secretkey"];

            // 2.Get the basket
            var basket = await _basketService.GetBasketAsync(basketId);

            if (basket?.BasketItems?.Count() == 0)
                return null;

            // 3.Get delivery method shipping price
            var shippingPrice = 0m;
            if (basket.DeliveryMethodID.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodID.Value);
                shippingPrice = deliveryMethod.Price;
                basket.ShippingPrice = shippingPrice;
            }

            // 4. check if the price of the products in the basket is not changed
            foreach (var item in basket.BasketItems)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                if (item.Price != product.Price)
                    item.Price = product.Price;
            }

            // 5. Create PaymentIntent object
            var service = new PaymentIntentService();
            PaymentIntent intent;

            // 6. Create or update PaymentIntent depending on the PaymentIntentId of the basket
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            // the basket does not have an associated PaymentIntentId yet, so it needs to create one
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + ((long)shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }

            else
            // the basket already has an associated PaymentIntentId that needs to be updated
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(item => item.Quantity * (item.Price * 100)) + ((long)shippingPrice * 100)
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            // 7.Update the basket
            basket = await _basketService.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<OrderResultDTO> UpdateOrderPaymentFailed(string PaymentIntentId)
        {
            var specs = new OrderWithPaymentIntentSpecifications(PaymentIntentId);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            if (order is null)
                return null;

            order.OrderStatus = OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Complete();

            var mappedOrder = _mapper.Map<OrderResultDTO>(order);

            return mappedOrder;
        }

        public async Task<OrderResultDTO> UpdateOrderPaymentSucceeded(string PaymentIntentId, string basketId)
        {
            var specs = new OrderWithPaymentIntentSpecifications(PaymentIntentId);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            if (order is null)
                return null;

            order.OrderStatus = OrderStatus.PaymentReceived;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.Complete();

            // delete basket data on payment success
            if (string.IsNullOrEmpty(basketId))
                await _basketService.DeleteBasketAsync(basketId);

            var mappedOrder = _mapper.Map<OrderResultDTO>(order);

            return mappedOrder;
        }
    }
}
