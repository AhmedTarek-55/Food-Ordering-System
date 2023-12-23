using AutoMapper;
using Core.Entities.Order_Entities;
using Core.Identity.Entities;

namespace Services.Services.Order_Service.DTO
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<AddressDTO, ShippingAddress>().ReverseMap();

            CreateMap<Order, OrderResultDTO>()
                .ForMember(dest => dest.DeliveryMethod, option => option.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, option => option.MapFrom(src => src.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDTO>()
               .ForMember(dest => dest.ProductItemId, option => option.MapFrom(src => src.ItemOrdered.ProductItemId))
               .ForMember(dest => dest.ProductName, option => option.MapFrom(src => src.ItemOrdered.ProductName))
               .ForMember(dest => dest.PictureURL, option => option.MapFrom<OrderItemUrlResolver>());

            CreateMap<OrderItemDTO, OrderItem>()
               .ForPath(dest => dest.ItemOrdered.ProductItemId, option => option.MapFrom(src => src.ProductItemId))
               .ForPath(dest => dest.ItemOrdered.ProductName, option => option.MapFrom(src => src.ProductName))
               .ForPath(dest => dest.ItemOrdered.PictureURL, option => option.MapFrom(src => src.PictureURL));
        }
    }
}