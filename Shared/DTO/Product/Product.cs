using System.ComponentModel.DataAnnotations;

namespace Shared.DTO.Product
{
    public class ProductForCreationDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string? Name { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? dealer { get; set; }
        public double? Weight { get; set; }
        public double? Price { get; set; }
        public DateOnly? Shipped_From { get; set; }
        public DateOnly? Shipped_To { get; set; }
        public string? Shipped_Address_From { get; set; }
        public string? Shipped_Address_To { get; set; }
        public Guid CategoryId { get; set; }
    }

    public class ProductForUpdateDto
    {
        public string? Name { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? dealer { get; set; }
        public double? Weight { get; set; }
        public DateOnly? Shipped_From { get; set; }
        public DateOnly? Shipped_To { get; set; }
        public double? Price { get; set; }
        public string? Shipped_Address_From { get; set; }
        public string? Shipped_Address_To { get; set; }
        public bool? In_Out { get; set; } = null; // In = true, Out = false
        public Guid CategoryId { get; set; }
        public Guid? ShelfId { get; set; }
        public Guid? WarehouseId { get; set; }
    }


    public class ProductDto
    {
        public Guid Id { get; set; }
        public string? CustomerName { get; set; }
        public string? Phone { get; set; }
        public string? dealer { get; set; }
        public Guid SerialNumber { get; set; }
        public string? Name { get; set; }
        public double? Weight { get; set; }
        public double? Price { get; set; }
        public DateOnly? Shipped_From { get; set; }
        public DateOnly? Shipped_To { get; set; }
        public string? Shipped_Address_From { get; set; }
        public string? Shipped_Address_To { get; set; }
        public bool? In_Out { get; set; } = null; // In = true, Out = false
        public string? CategoryName { get; set; }
        public Guid CategoryId { get; set; }
        public string? ShelfName { get; set; }
        public Guid? ShelfId { get; set; }
        public string? WarehouseName { get; set; }
        public Guid? WarehouseId { get; set; }
    }

    public class CategoryProductCountDto
    {
        public string? CategoryName { get; set; }
        public int TotalProducts { get; set; }
    }

}
