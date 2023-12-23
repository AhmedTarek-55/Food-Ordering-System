using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Services.Basket_Service;
using Services.Services.Basket_Service.DTO;

namespace API.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasketDTO>> GetBasketById(string id)
            => Ok(await _basketService.GetBasketAsync(id));

        [HttpPost]
        public async Task<ActionResult<CustomerBasketDTO>> UpdateBasketAsync(CustomerBasketDTO basket)
            => Ok(await _basketService.UpdateBasketAsync(basket));

        [HttpDelete]
        public async Task DeleteBasketById(string id)
            => Ok(await _basketService.DeleteBasketAsync(id));
    }
}
