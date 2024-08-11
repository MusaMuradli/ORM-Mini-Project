using ORM_Mini_Project.Enums;

namespace ORM_Mini_Project.Models;

public class Order : BaseEntity
{
    public int UserId { get; set; } 
    public DateTime OrderDate { get; set; } 
    public decimal TotalAmount { get; set; } 
    public OrderStatus Status { get; set; }

}
