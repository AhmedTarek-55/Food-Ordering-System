using AutoMapper;
using Infrastructure.Basket_Repository;
using Infrastructure.Basket_Repository.Basket_Entities;
using Services.Services.Basket_Service.DTO;

namespace Services.Services.Basket_Service
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
            => await _basketRepository.DeleteBasketAsync(basketId);

        public async Task<CustomerBasketDTO> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            if (basket is null)
                return new CustomerBasketDTO();

            var mappedBasket = _mapper.Map<CustomerBasketDTO>(basket);

            return mappedBasket;
        }

        public async Task<CustomerBasketDTO> UpdateBasketAsync(CustomerBasketDTO basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);

            var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);

            var mappedBasket = _mapper.Map<CustomerBasketDTO>(updatedBasket);

            return mappedBasket;
        }
    }
}
