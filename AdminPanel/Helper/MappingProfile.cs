using AdminPanel.Models.DeliveryMethodViewModels;
using AdminPanel.Models.ProductBrandViewModels;
using AdminPanel.Models.ProductTypeViewModels;
using AdminPanel.Models.ProductViewModels;
using AutoMapper;
using Core.Entities;

namespace AdminPanel.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();

            CreateMap<ProductBrand, ProductBrandViewModel>().ReverseMap();

            CreateMap<ProductType, ProductTypeViewModel>().ReverseMap();

            CreateMap<DeliveryMethod, DeliveryMethodViewModel>().ReverseMap();
        }
    }
}
