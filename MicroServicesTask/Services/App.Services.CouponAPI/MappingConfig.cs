
using App.Services.CouponApi.Entities;
using App.Services.CouponAPI.Entities.DTO;
using AutoMapper;

namespace App.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}