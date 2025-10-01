using Entities.Models;

namespace Contracts
{
    public interface IWarehouseRepository
    {
        Task<IEnumerable<Warehouse>> GetAllWarehousesAsync(bool trackChanges);
        Task<Warehouse> GetWarehouseByIdAsync(Guid warehouseId, bool trackChanges);
        void CreateWarehouse(Warehouse warehouse);
        void UpdateWarehouse(Warehouse warehouse);
        void DeleteWarehouse(Warehouse warehouse);
    }
}
