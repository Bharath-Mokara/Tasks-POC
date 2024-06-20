using App.Services.ShoppingCartApi.Entities.DTO;

namespace App.Services.ShoppingCartApi.ServiceContracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}