using Entities.Models;
using Shared.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.DTO.Shelf.Shelf;

namespace Contracts
{
    public interface IShelfRepository
    {
        Task<IEnumerable<Shelf>> GetAllShelfsAsync(bool trackChanges);
        Task<Shelf> GetShelfByIdAsync(Guid shelfId, bool trackChanges);
        Task<IEnumerable<ShelfCountDto>> GetShelfCountAsync(bool trackChanges);
        void CreateShelf(Shelf shelf);
        void UpdateShelf(Shelf shelf);
        void DeleteShelf(Shelf shelf);
    }
}
