using CareLite.Models.DTO;

namespace CareLite.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginRequest request);
        Task<string?> RegisterAsync(RegisterRequest request);
    }
}