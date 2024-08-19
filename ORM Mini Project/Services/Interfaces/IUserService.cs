using ORM_Mini_Project.DTOs.UserDTOs;
using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Services.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(RegisterUserDto registerUserDto);
    Task<User> LoginUserAsync(LoginUserDto loginUserDto);
    Task UpdateUserAsync(UpdateUserDto updateUserDto);
    Task<List<Order>> GetUserOrders(int id);
    Task<User> GetUserByFullNameAsync(string fullName);
}
