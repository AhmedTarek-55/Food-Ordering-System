using API.Handle_Responses;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Order_Service;
using Services.Services.Order_Service.DTO;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResultDTO>> CreateOrderAsync(OrderDTO orderDto)
        {
            var order = await _orderService.CreateOrderAsync(orderDto);

            if (order is null)
                return BadRequest(new ApiResponse(400, "Error occurred while creating the order"));

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderResultDTO>>> GetAllOrdersForUserAsync()
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderService.GetAllOrdersForUserAsync(email);

            if (orders is { Count: <= 0 })
                return Ok(new ApiResponse(200, "You don`t have orders yet"));

            return Ok(orders);
        }

        [HttpGet]
        public async Task<ActionResult<OrderResultDTO>> GetOrderByIdAsync(int Id)
        {
            var email = User?.FindFirstValue(ClaimTypes.Email);

            var order = await _orderService.GetOrderByIdAsync(Id, email);

            if (order is null)
                return Ok(new ApiResponse(200, $"There is no order with Id {Id}"));

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethodsAsync()
            => Ok(await _orderService.GetAllDeliveryMethodsAsync());
    }
}
