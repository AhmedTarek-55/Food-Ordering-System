using AutoMapper;
using Core.Entities.Order_Entities;
using Microsoft.Extensions.Configuration;

namespace Services.Services.Order_Service.DTO
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureURL))
                return $"{_configuration["BaseURL"]}{source.ItemOrdered.PictureURL}";

            return null;
        }
    }
}
