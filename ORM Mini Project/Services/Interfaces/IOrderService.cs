using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Services.Interfaces;

public  interface IOrderService
{
    Task CreateOrderAsync(Order order);
    Task<Order> GetOrderByIdAsync(int userId);
    Task UpdateOrderAsync(Order order);
    Task RemoveOrderAsync(int userId);
}
