using System.Runtime.CompilerServices;
using App.Services.ProductApi.Data;
using App.Services.ProductApi.Entities;
using App.Services.ProductApi.Entities.DTO;
using App.Services.ProductApiEntities.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Services.ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly BaseResponseDto _response;
        private readonly IMapper _mapper;
        public ProductController(ApplicationDbContext applicationDbContext,IMapper mapper){
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _response = new BaseResponseDto();
        }

        [HttpGet]
        public object? Get()
        {
            try{
                IEnumerable<Product> products = _applicationDbContext.Products;
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
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
                Product? product = _applicationDbContext.Products.ToList().First(product => product.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(product);
                _response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            
            return _response;
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public object? Post([FromBody] ProductDto productDto)
        {
            try{
                _applicationDbContext.Products.Add(
                    _mapper.Map<Product>(productDto)
                );
                _applicationDbContext.SaveChanges();

                _response.IsSuccess = true;
                _response.Message = "Product Added successfully";
                _response.Result = productDto;
            }
            catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


        [HttpPut]
        [Authorize(Roles = "Admin")]
        public object? Put([FromBody] ProductDto productDto)
        {
            try{
                _applicationDbContext.Products.Update(
                    _mapper.Map<Product>(productDto)
                );
                _applicationDbContext.SaveChanges();

                _response.IsSuccess = true;
                _response.Message = "Product Updated successfully";
                _response.Result = productDto;
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
        [Authorize(Roles = "Admin")]
        public object? Delete(int id)
        {
            try{
                Product productToBedeletede = _applicationDbContext.Products.First(product => product.ProductId == id);

                _applicationDbContext.Products.Remove(productToBedeletede);
                _applicationDbContext.SaveChanges();

                _response.IsSuccess = true;
                _response.Message = $"Product with id {id} deleted.";

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