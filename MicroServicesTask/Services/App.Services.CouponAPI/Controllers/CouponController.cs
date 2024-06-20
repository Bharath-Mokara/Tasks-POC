
using App.Services.CouponApi.Data;
using App.Services.CouponApi.Entities;
using App.Services.CouponAPI.Entities.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class CouponController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private BaseResponseDto _response;
        private IMapper _mapper;

        public CouponController(ApplicationDbContext applicationDbContext,IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _response = new BaseResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public object? Get()
        {
            try{
                IEnumerable<Coupon> coupons = _applicationDbContext.Coupons;
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
                _response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpGet("{id}")]
        public object? Get(int id)
        {
            try{
                Coupon? coupon = _applicationDbContext.Coupons.ToList().First(coupon => coupon.CouponId == id);
                _response.Result = _mapper.Map<CouponDto>(coupon);
                _response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/{couponCode}")]
        public object? GetByCode(string couponCode)
        {
            try{
                Coupon? coupon = _applicationDbContext.Coupons.FirstOrDefault(coupon => coupon.CouponCode.ToLower() == couponCode.ToLower());
                _response.Result = _mapper.Map<CouponDto>(coupon);
                if(coupon != null)
                    _response.IsSuccess = true;
                else 
                    _response.IsSuccess = false;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }


        [HttpPost]
        public object? Post([FromBody] CouponDto couponDto)
        {
            try{
                _applicationDbContext.Coupons.Add(
                    _mapper.Map<Coupon>(couponDto)
                );
                _applicationDbContext.SaveChanges();

                _response.IsSuccess = true;
                _response.Message = "Coupon Added successfully";
                _response.Result = couponDto;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


        [HttpPut]
        public object? Put([FromBody] CouponDto couponDto)
        {
            try{
                _applicationDbContext.Coupons.Update(
                    _mapper.Map<Coupon>(couponDto)
                );
                _applicationDbContext.SaveChanges();

                _response.IsSuccess = true;
                _response.Message = "Coupon Updated successfully";
                _response.Result = couponDto;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }

        [HttpDelete]
        [Route("{id}")]
        public object? Delete(int id)
        {
            try{
                Coupon couponToBedeletede = _applicationDbContext.Coupons.First(coupon => coupon.CouponId == id);

                _applicationDbContext.Coupons.Remove(couponToBedeletede);
                _applicationDbContext.SaveChanges();

                _response.IsSuccess = true;
                _response.Message = $"Coupon with id {id} deleted.";

            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}