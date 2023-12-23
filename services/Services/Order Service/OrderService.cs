using AutoMapper;
using Core.Entities;
using Core.Entities.Order_Entities;
using Infrastructure.Interfaces;
using Infrastructure.Specifications.Orders;
using Services.Services.Basket_Service;
using Services.Services.Basket_Service.DTO;
using Services.Services.Order_Service.DTO;
using Services.Services.Payment_Service;
using Product = Core.Entities.Product;

namespace Services.Services.Order_Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketService basketService, IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
        }

        public async Task<OrderResultDTO> CreateOrderAsync(OrderDTO orderDto)
        {
            // 1.Get the basket
            var basket = await _basketService.GetBasketAsync(orderDto.BasketId);

            if (basket?.BasketItems?.Count() == 0)
                return null;

            // 2. Fill OrderItems data from basket data
            var orderItems = new List<OrderItemDTO>();
            foreach (var item in basket.BasketItems)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productItemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(productItem.Price, item.Quantity, productItemOrdered);

                var mappedOrderItem = _mapper.Map<OrderItemDTO>(orderItem);
                orderItems.Add(mappedOrderItem);
            }

            // 3. Get Delivery Method
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(orderDto.DeliveryMethodId);

            // 4. Calculate SubTotal for all items in basket
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 5. Check if order already exists, then delete if it exists
            var specs = new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            if (existingOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                basket = await _paymentService.CreateOrUpdatePaymentIntent(orderDto.BasketId);
            }

            // 6. Get Payment Intent for the basket, if it doesn`t have one
            if (string.IsNullOrEmpty(basket?.PaymentIntentId))
                basket = await _paymentService.CreateOrUpdatePaymentIntent(orderDto.BasketId);

            // 7. Create Order
            var mappedShippingAddress = _mapper.Map<ShippingAddress>(orderDto.ShippingAddress);
            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);
            var order = new Order(orderDto.BuyerEmail, mappedShippingAddress, deliveryMethod, mappedOrderItems, subTotal, basket.PaymentIntentId);

            // 8. Add created order to database
            await _unitOfWork.Repository<Order>().Add(order);
            await _unitOfWork.Complete();

            var mappedOrder = _mapper.Map<OrderResultDTO>(order);
            return mappedOrder;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodsAsync()
            => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

        public async Task<IReadOnlyList<OrderResultDTO>> GetAllOrdersForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemsSpecifications(buyerEmail);

            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecificationsAsync(specs);

            var mappedOrders = _mapper.Map<List<OrderResultDTO>>(orders);

            return mappedOrders;
        }

        public async Task<OrderResultDTO> GetOrderByIdAsync(int Id, string buyerEmail)
        {
            var specs = new OrderWithItemsSpecifications(Id, buyerEmail);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecificationsAsync(specs);

            var mappedOrder = _mapper.Map<OrderResultDTO>(order);

            return mappedOrder;
        }
    }
}
