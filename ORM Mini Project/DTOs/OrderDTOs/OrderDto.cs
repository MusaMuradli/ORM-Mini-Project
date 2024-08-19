using ORM_Mini_Project.Enums;

namespace ORM_Mini_Project.DTOs.OrderDTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
