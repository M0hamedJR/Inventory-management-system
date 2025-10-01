using Contracts;
using Entities.Models;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private RepositoryContext _repoContext;
        private IProductRepository _product;
        private IShelfRepository _shelf;
        private ICategoryRepository _category;
        private IWarehouseRepository _warehouse;

        public RepositoryManager(RepositoryContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                    _product = new ProductRepository(_repoContext);
                return _product;
            }
        }

        public IShelfRepository Shelf
        {
            get
            {
                if (_shelf == null)
                    _shelf = new ShelfRepository(_repoContext);
                return _shelf;
            }
        }

        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                    _category = new CategoryRepository(_repoContext);
                return _category;
            }
        }

        public IWarehouseRepository Warehouse
        {
            get
            {
                if (_warehouse == null)
                    _warehouse = new WarehouseRepository(_repoContext);
                return _warehouse;
            }
        }

        public async Task SaveAsync()
        {
            await _repoContext.SaveChangesAsync();
        }
    }
}
