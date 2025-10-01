namespace Entities.Models
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public double? Capacity { get; set; }
        public List<Shelf>? Shelfs { get; set; }
        public List<Product>? Products { get; set; }
    }

}
