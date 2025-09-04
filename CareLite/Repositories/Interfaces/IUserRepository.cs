using CareLite.Models.Domain;

namespace CareLite.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
