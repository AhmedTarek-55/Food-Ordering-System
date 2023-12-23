using Services.Services.Basket_Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Basket_Service
{
    public interface IBasketService
    {
        Task<CustomerBasketDTO> GetBasketAsync(string basketId);
        Task<CustomerBasketDTO> UpdateBasketAsync(CustomerBasketDTO basket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
