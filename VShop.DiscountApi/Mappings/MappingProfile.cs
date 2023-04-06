using AutoMapper;
using VShop.DiscountApi.DTOs;
using VShop.DiscountApi.Models;

namespace VShop.DiscountApi.Mappings;

public class MappingProfile: Profile
{
    public MappingProfile() 
    {
        CreateMap<CouponDTO, Coupon>().ReverseMap();
    }
}
