using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Product;

namespace Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        private readonly Shelf shelf;
        public ProductRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateProduct(Product product)
        {
            Create(product);
        }

        public void DeleteProduct(Product product)
        {
            Delete(product);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).OrderBy(o => o.Name)
                 .Include(d => d.Category)
                 .Include(d => d.Shelf)
                 .Include(d => d.Warehouse)
                 .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(Guid productId, bool trackChanges)
        {
            return await FindByCondition(product => product.Id.Equals(productId), trackChanges)
                .Include(d => d.Category)
                .Include(d => d.Shelf)
                .Include(d => d.Warehouse)
                .FirstOrDefaultAsync();

        }
        public async Task<Product> GetProductBySerialNumberAsync(Guid serialNumber, bool trackChanges)
        {
            var query = RepositoryContext.Products.AsQueryable();

            if (!trackChanges)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(p => p.SerialNumber == serialNumber);
        }

        public async Task<IEnumerable<CategoryProductCountDto>> GetProductCountPerCategoryAsync(bool trackChanges)
        {
            var result = await FindAll(trackChanges)
                           .Include(d => d.Category)
                           .GroupBy(d => d.Category.Name)
                           .Select(g => new CategoryProductCountDto
                           {
                               CategoryName = g.Key,
                               TotalProducts = g.Count()
                           }).ToListAsync();

            return result;
        }

        public async Task<Product> GetProductWithDetailsAsync(Guid productId, bool trackChanges)
        {
            return await FindByCondition(product => product.Id.Equals(productId), trackChanges)
              .Include(d => d.Category)
              .FirstOrDefaultAsync();
        }

        public void UpdateProduct(Product product)
        {
            Update(product);
        }
    }
}