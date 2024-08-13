using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Contexts;
using ORM_Mini_Project.Models;
using ORM_Mini_Project.Services.Interfaces;

namespace ORM_Mini_Project.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService()
        {
            _context = new AppDbContext();
        }
        public async Task CreateUserAsync(User user)
        {
          _context.Add<User>(user);
           _context.SaveChanges();
        }

        public async Task DeleteUserAsync(int id)
        {
            _context.Remove(id);
            _context.SaveChanges();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public Task<List<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(int id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
