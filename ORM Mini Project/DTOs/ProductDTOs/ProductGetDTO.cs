namespace ORM_Mini_Project.DTOs.ProductDTOs
{
    public record ProductGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
    }
}
