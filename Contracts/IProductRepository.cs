using Entities.Models;
using Shared.DTO.Product;

namespace Contracts
{
    public interface IProductRepository
    {

        Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges);
        Task<Product> GetProductByIdAsync(Guid productId, bool trackChanges);
        Task<Product> GetProductBySerialNumberAsync(Guid productId, bool trackChanges);
        Task<Product> GetProductWithDetailsAsync(Guid productId, bool trackChanges);
        Task<IEnumerable<CategoryProductCountDto>> GetProductCountPerCategoryAsync(bool trackChanges);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}
