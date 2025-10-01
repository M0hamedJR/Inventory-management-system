namespace Contracts
{
    public interface IRepositoryManager
    {
        IProductRepository Product { get; }
        IShelfRepository Shelf { get; }
        ICategoryRepository Category { get; }
        IWarehouseRepository Warehouse { get; }
        Task SaveAsync();
    }
}
