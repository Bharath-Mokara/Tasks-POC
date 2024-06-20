using App.Services.ShoppingCartApi.Entities.DTO;
using App.Services.ShoppingCartApi.ServiceContracts;
using Newtonsoft.Json;

namespace App.Services.ShoppingCartApi.Services
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            
            //create a couponClient using httpClientFactory
            //fetch the response from coupon service
            //Deserialize the response

            var client = _httpClientFactory.CreateClient("CouponClient");
            var apiResponse = await client.GetAsync($"/api/Coupon/GetByCode/{couponCode}");

            var apiContent = await apiResponse.Content.ReadAsStringAsync();

            var response = JsonConvert.DeserializeObject<BaseResponseDto>(apiContent);

            if(response.IsSuccess) 
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
            }

            return new CouponDto();

            
        }
    }
}