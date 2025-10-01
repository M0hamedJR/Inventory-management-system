using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DTO.Product;
using Shared.DTO.Shelf;
using static Shared.DTO.Shelf.Shelf;
using Shelf = Entities.Models.Shelf;
namespace Repository
{
    public class ShelfRepository : RepositoryBase<Shelf>, IShelfRepository
    {
        public ShelfRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateShelf(Shelf shelf)
        {
            Create(shelf);
        }

        public void DeleteShelf(Shelf shelf)
        {
            Delete(shelf);
        }

        public async Task<IEnumerable<Shelf>> GetAllShelfsAsync(bool trackChanges)
        {
            return await FindAll(trackChanges).OrderBy(s => s.Id).Include(d => d.Category)
                 .Include(d => d.Warehouse).ToListAsync();
        }

        public async Task<Shelf> GetShelfByIdAsync(Guid shelfId, bool trackChanges)
        {
            return await FindByCondition(shelf => shelf.Id.Equals(shelfId), trackChanges)
            .Include(d => d.Category)
            .Include(d => d.Warehouse)
            .FirstOrDefaultAsync();
        }

        public void UpdateShelf(Shelf shelf)
        {
            Update(shelf);
        }
        public async Task<IEnumerable<ShelfCountDto>> GetShelfCountAsync(bool trackChanges)
        {
            // Group by category and get counts
            var categoryCounts = await FindAll(trackChanges)
                .Include(d => d.Warehouse) // Assuming there is a navigation property 'Category' with 'Name' in Shelf entity
                .GroupBy(d => d.Warehouse.Name)
                .Select(g => new ShelfCountDto
                {
                    WarehouseName = g.Key,
                    TotalShelfs = g.Count(),
                    AvailableTotalShelfs = g.Count(s => s.IsAvailable),
                    AvailableShelfsMinSize = g.Count(s => s.IsAvailable && s.Category.Name == "Min"),
                    AvailableShelfsMediumSize= g.Count(s => s.IsAvailable && s.Category.Name == "Medium"),
                    AvailableShelfsMaxSize = g.Count(s => s.IsAvailable && s.Category.Name == "Max"),
                }).ToListAsync();

            return categoryCounts;
        }

    }
}
