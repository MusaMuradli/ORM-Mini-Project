using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Contexts;
using ORM_Mini_Project.DTOs.OrderDTOs;
using ORM_Mini_Project.Enums;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Services.Interfaces;


namespace ORM_Mini_Project.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService()
        {
        }

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new NotFoundException("Order not found!");
            }

            if (order.Status == Order.OrderStatus.Cancelled)
            {
                throw new OrderAlreadyCancelledException("Order has already been cancelled!");
            }

            order.Status = (Order.OrderStatus)OrderStatus.Cancelled;
            
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task CompleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new NotFoundException("Order not found!");
            }

            if (order.Status == Order.OrderStatus.Cancelled)
            {
                throw new OrderAlreadyCancelledException("Order has already been completed!");
            }

            order.Status = (Order.OrderStatus)OrderStatus.Completed;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateOrderAsync(int userId)
        {
             var context = new AppDbContext();

            Order order = new()
            {
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0,
                UserId = userId,
                Status = Order.OrderStatus.Pending
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            return order.Id;
        }

        public async Task CreateOrderAsync(OrderDto orderDto)
        {

            var context = new AppDbContext();

            Order order = new()
            {
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0,
                UserId = orderDto.UserId,
                Status = Order.OrderStatus.Pending
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

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

        public async Task<List<OrderDto>> GetOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            List<OrderDto> ordersList = new List<OrderDto>();

            foreach (var order in orders)
            {
                OrderDto dto = new()
                {
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = (ORM_Mini_Project.Enums.OrderStatus)order.Status
                };

                ordersList.Add(dto);
            }

            return ordersList;
        }

    }
}
