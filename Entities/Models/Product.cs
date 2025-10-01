using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? dealer { get; set; }
        public Guid SerialNumber { get; set; }
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }
        public double? Weight { get; set; }
        public double? Price { get; set; }
        public bool? In_Out { get; set; } = null; // In = true, Out = false
        public DateOnly? Shipped_From { get; set; }
        public DateOnly? Shipped_To { get; set; }
        public string? Shipped_Address_From { get; set; }
        public string? Shipped_Address_To { get; set; }
        public Guid? ShelfId { get; set; }
        public Shelf? Shelf { get; set; }
        public Guid? WarehouseId { get; set; }
        public Warehouse? Warehouse { get; set; } 
    }
}
