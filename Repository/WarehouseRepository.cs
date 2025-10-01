using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class WarehouseRepository : RepositoryBase<Warehouse>, IWarehouseRepository
    {
        public WarehouseRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateWarehouse(Warehouse warehouse)
        {
            var existingWarehouses = RepositoryContext.Warehouses
                .ToList();

            char nextLetter = 'A'; // Start from 'A' if no warehouses exist for this name

            if (existingWarehouses.Any())
            {
                // Extract the last letters of all matching warehouses
                var lastLetters = existingWarehouses
                    .Select(w => w.Name.Last()) // Get last character
                    .Where(ch => ch >= 'A' && ch <= 'Z') // Ensure it's a letter
                    .OrderBy(ch => ch) // Sort from A-Z
                    .ToList();

                // Find the next available letter
                for (char letter = 'A'; letter <= 'Z'; letter++)
                {
                    if (!lastLetters.Contains(letter))
                    {
                        nextLetter = letter;
                        break;
                    }
                }

                // If all letters from A-Z are used, throw an exception
                if (nextLetter > 'Z')
                {
                    throw new Exception("Warehouse naming limit reached (A-Z).");
                }
            }

            // Create the new warehouse with the next available letter
            var newWarehouse = new Warehouse
            {
                Name = $"{warehouse.Name} {nextLetter}",
                Location = warehouse.Location,
                Capacity = warehouse.Capacity
            };

            Create(newWarehouse);
            RepositoryContext.SaveChanges();
        }

        public void DeleteWarehouse(Warehouse warehouse)
        {
            Delete(warehouse);
        }

        public async Task<IEnumerable<Warehouse>> GetAllWarehousesAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
              .OrderBy(b => b.Name)
              .Include(d => d.Products)
              .ThenInclude(dev => dev.Category)
              .ToListAsync();
        }

        public async Task<Warehouse> GetWarehouseByIdAsync(Guid warehouseId, bool trackChanges)
        {
            return await FindByCondition(warehouse => warehouse.Id.Equals(warehouseId), trackChanges)
            .OrderBy(b => b.Name)
            .Include(p => p.Products)
            .ThenInclude(dev => dev.Category)
            .FirstOrDefaultAsync();
        }

        public void UpdateWarehouse(Warehouse warehouse)
        {
            var existingWarehouses = RepositoryContext.Warehouses
               .ToList();
            var existWarehouse = RepositoryContext.Warehouses
               .FirstOrDefault(w => w.Id == warehouse.Id);
            char nextLetter = 'A'; // Start from 'A' if no warehouses exist for this name

            if (existingWarehouses.Any())
            {
                // Extract the last letters of all matching warehouses
                var lastLetters = existingWarehouses
                    .Select(w => w.Name.Last()) // Get last character
                    .Where(ch => ch >= 'A' && ch <= 'Z') // Ensure it's a letter
                    .OrderBy(ch => ch) // Sort from A-Z
                    .ToList();

                // Find the next available letter
                for (char letter = 'A'; letter <= 'Z'; letter++)
                {
                    if (!lastLetters.Contains(letter))
                    {
                        nextLetter = letter;
                        break;
                    }
                }

                // If all letters from A-Z are used, throw an exception
                if (nextLetter > 'Z')
                {
                    throw new Exception("Warehouse naming limit reached (A-Z).");
                }
            }

            existWarehouse.Name = $"{warehouse.Name} {nextLetter}";
            existWarehouse.Location = warehouse.Location;
            existWarehouse.Capacity = warehouse.Capacity;
            Update(existWarehouse);
            RepositoryContext.SaveChanges();
        }
    }
}