using AutoMapper;
using SaleStreets_Back_end.Models;
using SaleStreets_Back_end.Models.dtos;
using SaleStreets_Back_end.Services;

namespace SaleStreets_Back_end.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>();

            CreateMap<Product, GetProductDto>()
                .ForMember(dest => dest.Publisher, src => src.MapFrom(src => src.Publisher.Email));
                
        }
    }


 
}
