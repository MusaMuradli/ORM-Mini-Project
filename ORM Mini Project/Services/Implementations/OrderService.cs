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
            _context = new AppDbContext();
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


        public async Task CreateOrderAsync(OrderDto orderDto)
        {

            var context = new AppDbContext();

            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == orderDto.ProductId);

            if (product is null)
                throw new NotFoundException("Product is not found");

            Order order = new()
            {
                OrderDate = DateTime.UtcNow,
                TotalAmount = product.Price * orderDto.Quantity,
                UserId = orderDto.UserId,
                Status = Order.OrderStatus.Pending
            };


            OrderDetail orderDetail = new()
            {
                Order=order,
                PricePerItem = product.Price,
                Quantity = orderDto.Quantity,
                ProductId = product.Id
            };


            order.OrderDetails.Add(orderDetail);
            

            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                throw new NotFoundException("Order not found.");
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();


        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
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

            List<OrderDto> ordersList = new();

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
