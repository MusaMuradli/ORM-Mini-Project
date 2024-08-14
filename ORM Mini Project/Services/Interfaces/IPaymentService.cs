using ORM_Mini_Project.DTOs.PaymentDTOs;
using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Services.Interfaces;

public interface IPaymentService
{
    Task MakePayment (Payment payment);
    Task<List<PaymentDto>> GetAllPayment();
}

