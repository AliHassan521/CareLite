using CareLite.Models.Domain;

namespace CareLite.Repositories.Interfaces
{
    public interface IUserRepository
    {
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> ValidateCredentialsAsync(string username, string passwordHash);
    Task InsertAuditLogAsync(Guid correlationId, int? userId, string action, string description);
    }
}
