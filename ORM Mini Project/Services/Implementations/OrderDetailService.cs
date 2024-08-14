using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Contexts;
using ORM_Mini_Project.DTOs.OrderDetailDTOs;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Services.Interfaces;

namespace ORM_Mini_Project.Services.Implementations
{
    public class OrderDetailServiceL : IOrderDetailService
    {
        private readonly AppDbContext _context;
        public OrderDetailServiceL()
        {
            _context = new AppDbContext();
        }
        public async Task AddOrderDetailAsync(OrderDetailDto orderDetailDto)
        {
            if (orderDetailDto.Quantity <= 0 || orderDetailDto.PricePerItem < 0)
                throw new InvalidOrderDetailException("Invalid order detail.");
            var order = await _context.Orders.FindAsync(orderDetailDto.OrderId);
            if (order == null)
                throw new NotFoundException("Order not found.");
            var product = await _context.Products.FindAsync(orderDetailDto.ProductId);
            if (product == null)
                throw new NotFoundException("Product not found.");

            var orderDetail = new OrderDetail
            {
                OrderId = orderDetailDto.OrderId,
                ProductId = orderDetailDto.ProductId,
                Quantity = orderDetailDto.Quantity,
                PricePerItem = orderDetailDto.PricePerItem
            };

            _context.OrderDetails.Add(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderDetailDto>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                throw new InvalidOrderDetailException("Order not found.");

            var orderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .ToListAsync();

            return orderDetails.Select(od => new OrderDetailDto
            {
                OrderId = od.OrderId,
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                PricePerItem = od.PricePerItem
            }).ToList();
        }
    }
}
