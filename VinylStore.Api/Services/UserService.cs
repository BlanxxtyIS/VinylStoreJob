using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using VinylStore.Api.Persistence;
using VinylStore.Api.Persistence.Entities;
using VinylStore.Shared.AuthModel;

namespace VinylStore.Api.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(MyRegisterRequest request);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> ValidateUserCredentialsAsync(string email, string password);
        Task UpdateRefreshTokenAsync(User user, string refreshToken);
    }

    public class UserService : IUserService
    {
        private readonly VinylStoreContext _context;

        public UserService(VinylStoreContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(MyRegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new Exception("User with this email already exists");
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await GetUserByEmailAsync(email);
            if (user == null) return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task UpdateRefreshTokenAsync(User user, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
        }
    }
}
