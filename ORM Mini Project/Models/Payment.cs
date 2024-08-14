namespace ORM_Mini_Project.Models;

public class Payment:BaseEntity
{
    public int OrderId { get; set; }
    public decimal Amount { get; set; } 
    public DateTime PaymentDate { get; set; }
    public Order Order { get; set; }


}
