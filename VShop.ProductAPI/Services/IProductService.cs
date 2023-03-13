using VShop.ProductAPI.DTOs;

namespace VShop.ProductAPI.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProducts();
    Task<ProductDTO> GetProductById(int id);
    Task AddProduct(ProductDTO productDto);
    Task UpdateProduct(ProductDTO productDto);
    Task DeleteProduct(int id);
}
