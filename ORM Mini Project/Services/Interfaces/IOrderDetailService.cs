using ORM_Mini_Project.DTOs.OrderDetailDTOs;

namespace ORM_Mini_Project.Services.Interfaces;

public interface IOrderDetailService
{
    Task AddOrderDetailAsync(OrderDetailDto orderDetailDto);
    Task<List<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId);
}
