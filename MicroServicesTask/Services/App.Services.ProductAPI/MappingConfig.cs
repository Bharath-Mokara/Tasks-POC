using App.Services.ProductApi.Entities;
using App.Services.ProductApiEntities.DTO;
using AutoMapper;

namespace App.Services.ProductApi{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config => {
                config.CreateMap<Product,ProductDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}