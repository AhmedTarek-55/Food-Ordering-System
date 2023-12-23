using AutoMapper;
using Infrastructure.Basket_Repository.Basket_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.Basket_Service.DTO
{
    public class BasketProfile : Profile
    {
        public BasketProfile() 
        {
            CreateMap<BasketItem,BasketItemDTO>().ReverseMap();
            CreateMap<CustomerBasket, CustomerBasketDTO>().ReverseMap();
        }
    }
}
