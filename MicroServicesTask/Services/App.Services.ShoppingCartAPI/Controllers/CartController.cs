using App.Services.ShoppingCartApi.Data;
using App.Services.ShoppingCartApi.Entities;
using App.Services.ShoppingCartApi.Entities.DTO;
using App.Services.ShoppingCartApi.ServiceContracts;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Services.ShoppingCartApi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private BaseResponseDto _response;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;

        public CartController(ApplicationDbContext applicationDbContext,IMapper mapper,IProductService productService,ICouponService couponService)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _response = new BaseResponseDto();
            _productService = productService;
            _couponService = couponService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<BaseResponseDto> GetCart(string userId)
        {
            try{
                CartDto cart = new()
                {
                    //fetching the cart header based on user Id where the cartItems will be a List<CartDetail>
                    CartHeader = _mapper.Map<CartHeaderDto>(
                        _applicationDbContext.CartHeaders.FirstOrDefault(cartHeader => cartHeader.UserId == userId)
                    )
                };
 
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(
                    _applicationDbContext.CartDetails.Where(cartDetail => cartDetail.CartHeaderId == cart.CartHeader.CartHeaderId)
                );

                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

                foreach(var item in cart.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(productDto => productDto.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += item.Count * item.Product.Price;
                }

                //Apply coupon if any
                if(!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);
                    if(coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;
                _response.IsSuccess = true;
                _response.Message = "User cart fetched";
            }
            catch(Exception ex){
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }

            return _response;
        }

        [HttpPost("CartUpSert")]
        public async Task<BaseResponseDto> CartUpSert([FromBody]CartDto cartDto)
        {
            /*Cart Functionality
            Both Insert and Update gonna be handled by the same action method.
            Steps:
            1: User adds first item to the cart
                - Create Cart Header
                - Create Cart Details
            2: User adds a new item in to the shoppingCart (User already has few other items in cart)
                - Find Cart Header
                - Add Cart Details under same cart header Id
            3: User updates quantity of an exisitng item in the cart
                - Find Details
                - Update Count in Cart Details
            */

            try{
                CartHeader? cartHeaderFromDb = await _applicationDbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(cartHeader => cartHeader.UserId == cartDto.CartHeader.UserId);
                if(cartHeaderFromDb == null)
                {
                    //Create a cartHeader and Details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _applicationDbContext.CartHeaders.Add(cartHeader);
                    await _applicationDbContext.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                    _applicationDbContext.CartDetails.Add(cartDetails);
                    await _applicationDbContext.SaveChangesAsync();

                    _response.Message = "Item added to cart";

                }
                else{
                    //if header is not null
                    //check if details has same product
                    var cartDetailsFromDb = await _applicationDbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(cartDetail => 
                        cartDetail.CartHeaderId == cartHeaderFromDb.CartHeaderId &&
                        cartDetail.ProductId == cartDto.CartDetails.First().ProductId);

                    if(cartDetailsFromDb == null)
                    {
                        //create cartDetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                        _applicationDbContext.CartDetails.Add(cartDetails);
                        await _applicationDbContext.SaveChangesAsync();

                        _response.Message = "Item added to cart";

                    }
                    else{
                        //Update the count in cart Details
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;

                        CartDetails updatedCartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());

                        _applicationDbContext.CartDetails.Update(updatedCartDetails);
                        await _applicationDbContext.SaveChangesAsync();

                        _response.Message = "cart updated successfully";

                    }
                }

                _response.Result = cartDto;
                _response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
            }

            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<BaseResponseDto> RemoveCart(int CartDetailsId)
        {
            
            try{
                CartDetails cartDetails = _applicationDbContext.CartDetails.First(cartDetail => cartDetail.CartDetailsId == CartDetailsId);

                _applicationDbContext.CartDetails.Remove(cartDetails);

                int totalCountOfCartItems = _applicationDbContext.CartDetails.Where(cartDetail => cartDetail.CartHeaderId == cartDetails.CartHeaderId).Count();

                if(totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await _applicationDbContext.CartHeaders.FirstOrDefaultAsync(cartHeader => cartHeader.CartHeaderId == cartDetails.CartHeaderId);
                    _applicationDbContext.CartHeaders.Remove(cartHeaderToRemove);
                }

                await _applicationDbContext.SaveChangesAsync();
                _response.IsSuccess = true;
                _response.Message = "Item deleted from cart";
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
            }

            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody]CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _applicationDbContext.CartHeaders.FirstAsync(cartHeader => cartHeader.UserId == cartDto.CartHeader.UserId);
                cartFromDb.CouponCode = cartDto.CartHeader.CouponCode;

                _applicationDbContext.CartHeaders.Update(cartFromDb);
                await _applicationDbContext.SaveChangesAsync();

                _response.IsSuccess = true;
                _response.Message = "Coupon applied successfully";
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.ToString();
            }

            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody]CartDto cartDto)
        {
            try
            {
                var cartFromDb = await _applicationDbContext.CartHeaders.FirstAsync(cartHeader => cartHeader.CartHeaderId == cartDto.CartHeader.CartHeaderId);
                if(cartFromDb != null){
                    cartFromDb.CouponCode = string.Empty;
                    _applicationDbContext.CartHeaders.Update(cartFromDb);
                    await _applicationDbContext.SaveChangesAsync();

                    _response.IsSuccess = true;
                    _response.Message = "Coupon removed successfully";
                }
                
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message.ToString();
            }

            return _response;
        }


   }
}