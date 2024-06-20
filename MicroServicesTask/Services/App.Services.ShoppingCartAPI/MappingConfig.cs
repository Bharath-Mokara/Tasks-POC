using App.Services.ShoppingCartApi.Entities;
using App.Services.ShoppingCartApi.Entities.DTO;
using AutoMapper;

namespace App.Services.ShoppingCartApi{
    public class MappingConfig{
        public static MapperConfiguration RegisterMaps(){
            var mappingConfig = new MapperConfiguration(config =>{

                config.CreateMap<CartHeader,CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails,CartDetailsDto>().ReverseMap();
                
            });
            return mappingConfig;
        }

    }
}