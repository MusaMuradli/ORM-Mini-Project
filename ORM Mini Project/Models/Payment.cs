namespace ORM_Mini_Project.Models;

public class Payment:BaseEntity
{
    public int OrderId { get; set; } //Ödəniş edilən sifarişin ID-si
    public decimal Amount { get; set; } // Ödəniş məbləği
    public DateTime PaymentDate { get; set; } // Ödəniş tarixi


}
