using ORM_Mini_Project.Models;

namespace ORM_Mini_Project.Services.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllUsersAsync();
    Task<List<User>> GetUsersAsync();
    Task CreateUserAsync (User user);
    Task DeleteUserAsync(int id);
    Task UpdateUserAsync (int id, User user);
}
