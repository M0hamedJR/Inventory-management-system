using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTO.Shelf
{
    public class Shelf
    {
        public class ShelfForCreationDto
        {
            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; }
            public bool IsAvailable { get; set; } = true;
            public Guid Warehouse_Id { get; set; }
            public Guid CategoryId { get; set; }
        }

        public class ShelfForUpdateDto
        {
            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Is Available is required")]
            public bool IsAvailable { get; set; } = true;
            public Guid Warehouse_Id { get; set; }
            public Guid CategoryId { get; set; }
        }

        public class ShelfDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public bool IsAvailable { get; set; } = true;
            public string? WarehouseName { get; set; }
            public Guid Warehouse_Id { get; set; }
            public string? CategoryName { get; set; }
            public Guid CategoryId { get; set; }
        }
        public class ShelfCountDto
        {
            public int TotalShelfs { get; set; }
            public int AvailableTotalShelfs { get; set; }
            public string? WarehouseName { get; set; }
            public int AvailableShelfsMinSize { get; set; }
            public int AvailableShelfsMediumSize { get; set; }
            public int AvailableShelfsMaxSize { get; set; }
        }
    }
}
