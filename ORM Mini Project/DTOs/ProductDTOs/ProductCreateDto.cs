namespace ORM_Mini_Project.DTOs.ProductDTOs
{
    public record ProductCreateDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
    }
}
