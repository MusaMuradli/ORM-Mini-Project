using ORM_Mini_Project.DTOs.OrderDTOs;
using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Services.Interfaces;

public  interface IOrderService
{
    Task CreateOrderAsync(OrderDto orderDto);
    Task UpdateOrderAsync(int orderId, OrderDto orderDto);
    Task DeleteOrderAsync(int orderId);
    Task<OrderDto> GetOrderByIdAsync(int orderId);
    Task<List<OrderDto>> GetOrdersAsync();
}
