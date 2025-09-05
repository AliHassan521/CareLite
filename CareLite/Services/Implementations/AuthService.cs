using CareLite.Models.DTO;
using CareLite.Models.Domain;
using CareLite.Repositories.Interfaces;
using CareLite.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace CareLite.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(User? user, Guid correlationId, string error)> SignInAsync(LoginRequest request)
        {
            var correlationId = Guid.NewGuid();
            var user = await _userRepository.ValidateCredentialsAsync(request.Username, request.Password);
            if (user == null)
            {
                await _userRepository.InsertAuditLogAsync(correlationId, null, "SignInFailed", $"Failed sign-in for {request.Username}");
                return (null, correlationId, "Invalid credentials.");
            }
            await _userRepository.InsertAuditLogAsync(correlationId, user.UserId, "SignInSuccess", $"User {user.Username} signed in.");
            return (user, correlationId, string.Empty);
        }

        public async Task<Guid> SignOutAsync(int? userId)
        {
            var correlationId = Guid.NewGuid();
            await _userRepository.InsertAuditLogAsync(correlationId, userId, "SignOut", "User signed out.");
            return correlationId;
        }
    }
}
