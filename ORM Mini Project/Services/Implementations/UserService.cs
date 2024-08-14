using Microsoft.EntityFrameworkCore;
using ORM_Mini_Project.Contexts;
using ORM_Mini_Project.DTOs.UserDTOs;
using ORM_Mini_Project.Exceptions;
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

        public async Task RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerUserDto.Email);

            if (existUser != null)
            {
                throw new InvalidUserInformationException("Bu e-poçt ünvanı artıq istifade olunub.");
            }
            var newUser = new User
            {
                FullName = registerUserDto.FullName,
                Email = registerUserDto.Email,
                Password = registerUserDto.Password,
                Address = registerUserDto.Address
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }
        private string HashPassword(string password)
        {

            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public async Task<User> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUserDto.Email);
            if (user == null)
            {
                throw new UserAuthenticationException("İstifadeçi tapılmadı.");
            }
            if (user.Password != loginUserDto.Password)
            {
                throw new UserAuthenticationException("Yanlış şifre.");
            }
            return user;
        }


        public async Task<List<Order>> GetUserOrders(int userId)
        {
            var user = await _context.Users.Include(x => x.Orders).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new NotFoundException($"User not found with userId:{userId}");

            if (user.Orders.Count == 0)
                Console.WriteLine("This user Hasn't orders!");

            return user.Orders;
        }
        
        public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updateUserDto.Id);
            if (user == null)
            {
                throw new NotFoundException($"Bu {updateUserDto.Id} ID-li istifadуçi tapılmadı.");
            }
            user.FullName = updateUserDto.FullName;
            user.Email = updateUserDto.Email;
            user.Password = HashPassword(updateUserDto.Password);
            user.Address = updateUserDto.Address;
            await _context.SaveChangesAsync();
        }
    }
}
