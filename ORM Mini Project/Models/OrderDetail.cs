namespace ORM_Mini_Project.Models;

public class OrderDetail: BaseEntity
{
    public  int  OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; } //Sifariş edilmiş məhsulun sayı
    public decimal PricePerItem { get; set; } //Məhsulun hər biri üçün qiymət.

}
