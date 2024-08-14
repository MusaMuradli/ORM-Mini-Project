using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Contexts;
using ORM_Mini_Project.DTOs.OrderDTOs;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Services.Interfaces;

namespace ORM_Mini_Project.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateOrderAsync(OrderDto orderDto)
        {
            if (orderDto.TotalAmount < 0)
                throw new InvalidOrderException("Order amount cannot be negative.");
            var user = await _context.Users.FindAsync(orderDto.UserId);
            if (user == null)
                throw new NotFoundException("User not found.");
            var order = new Order
            {
                UserId = orderDto.UserId,
                OrderDate = orderDto.OrderDate,
                TotalAmount = orderDto.TotalAmount,
                Status = Order.OrderStatus.Pending
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o=>o.Id==orderId);
            if (order==null)
                throw new NotFoundException("Order not found.");
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();


        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o=>o.Id==orderId);
            if (order == null)
                throw new NotFoundException("Order not found.");
            return new OrderDto
            {
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = (Enums.OrderStatus)order.Status
            };
        }

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            var orders = await _context.Orders.ToListAsync();
            return orders.Select(order => new OrderDto
            {
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = (Enums.OrderStatus)order.Status
            }).ToList();
        }

        public async Task UpdateOrderAsync(int orderId, OrderDto orderDto)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order==null)
                throw new NotFoundException("Order not found.");
            order.UserId= orderId;
            order.OrderDate= orderDto.OrderDate;
            order.TotalAmount= orderDto.TotalAmount;
            order.Status = (Order.OrderStatus)orderDto.Status;
            await _context.SaveChangesAsync();
        }
    }
}
