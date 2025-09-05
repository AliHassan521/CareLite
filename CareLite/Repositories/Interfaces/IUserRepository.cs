using CareLite.Models.Domain;


namespace CareLite.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task AuditLogAsync(Guid correlationId, int? userId, string action, string description);
        Task<User> CreateUserAsync(User user, string password);
    }
}
