using CareLite.Models.DTO;
using CareLite.Models.Domain;
using System;
using System.Threading.Tasks;

namespace CareLite.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(User? user, Guid correlationId, string error)> SignInAsync(LoginRequest request);
        Task<Guid> SignOutAsync(int? userId);
    }
}
