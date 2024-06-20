using App.Services.ShoppingCartApi.Entities.DTO;
using App.Services.ShoppingCartApi.ServiceContracts;
using Newtonsoft.Json;

namespace App.Services.ShoppingCartApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("ProductClient");
            var apiResponse = await client.GetAsync($"/api/Product");

            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<BaseResponseDto>(apiContent);

            if(response.IsSuccess) 
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(response.Result));
            }

            return new List<ProductDto>();
        }
    }
}