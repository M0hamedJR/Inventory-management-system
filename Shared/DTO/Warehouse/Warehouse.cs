
using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Warehouse
{
    public class WarehouseForCreationDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string? Name { get; set; }

        [StringLength(100, ErrorMessage = "Location can't be longer than 100 characters")]
        public string? Location { get; set; }
        public int? Capacity { get; set; }
    }

    public class WarehouseForUpdateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string? Name { get; set; }

        [StringLength(100, ErrorMessage = "Location can't be longer than 100 characters")]
        public string? Location { get; set; }
        public int? Capacity { get; set; }
    }

    public class WarehouseDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public int? Capacity { get; set; }
    }
}
