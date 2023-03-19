using AutoMapper;
using VShop.CartApi.DTOs;
using VShop.CartApi.Models;

namespace VShop.CartApi.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile() 
    {
        CreateMap<CartDTO,Cart>().ReverseMap();
        CreateMap<CartHeaderDTO,CartHeader>().ReverseMap();
        CreateMap<CartItemDTO,CartItem>().ReverseMap();
        CreateMap<ProductDTO,Product>().ReverseMap();
    }
}
