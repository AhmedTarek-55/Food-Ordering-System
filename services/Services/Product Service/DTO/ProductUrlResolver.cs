using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace Services.Services.Product_Service.DTO
{
    internal class ProductUrlResolver : IValueResolver<Product, ProductResultDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, ProductResultDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return _configuration["BaseURL"] + source.PictureUrl;

            return null;
        }
    }
}
