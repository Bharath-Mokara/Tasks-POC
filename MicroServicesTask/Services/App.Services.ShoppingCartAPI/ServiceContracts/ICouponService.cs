using App.Services.ShoppingCartApi.Entities.DTO;

namespace App.Services.ShoppingCartApi.ServiceContracts
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}