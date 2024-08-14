using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Contexts;
using ORM_Mini_Project.DTOs.PaymentDTOs;
using ORM_Mini_Project.Exceptions;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Services.Interfaces;

namespace ORM_Mini_Project.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;
        public PaymentService()
        {
            _context = new AppDbContext();
        }
       

        public async Task MakePayment(Payment payment)
        {
            if (payment.Amount <= 0)
            {
                throw new InvalidPaymentException("Payment amount must be greater than zero.");
            }
            var order = await _context.Orders.FindAsync(payment.OrderId);
            if (order == null)
            {
                throw new NotFoundException("Order not found.");
            }

            payment.Order = order;
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }
        public async Task<List<PaymentDto>> GetAllPayment()
        {
            return await _context.Payments
            .Select(p => new PaymentDto
            {
                OrderId = p.OrderId,
                Order = p.Order,
                Amount = p.Amount
            })
            .ToListAsync();
        }
    }
}
