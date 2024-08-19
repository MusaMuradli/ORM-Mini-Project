using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.DTOs.OrderDetailDTOs
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerItem { get; set; }
        public int UserId { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
