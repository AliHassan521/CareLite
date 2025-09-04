using CareLite.Data;
using CareLite.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CareLite.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CareLiteDbContext _context;
        public UserRepository(CareLiteDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
