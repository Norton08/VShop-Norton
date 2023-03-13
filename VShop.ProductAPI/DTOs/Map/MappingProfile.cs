using AutoMapper;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VShop.ProductAPI.Models;

namespace VShop.ProductAPI.DTOs.Map
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category,CategoryDTO>().ReverseMap();

            CreateMap<ProductDTO, Product>();

            CreateMap<Product,ProductDTO>()
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        }

    }
}
