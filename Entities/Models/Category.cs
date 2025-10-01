namespace Entities.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int ProductCount { get; set; }
        public List<Product>? Products { get; set; }
        public List<Shelf>? Shelfs { get; set; }
    }
}
