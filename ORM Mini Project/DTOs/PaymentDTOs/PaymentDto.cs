using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.DTOs.PaymentDTOs
{
    public class PaymentDto
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public decimal Amount { get; set; }
    }
}
