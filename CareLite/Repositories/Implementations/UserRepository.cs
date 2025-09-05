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

        public async Task<User?> ValidateCredentialsAsync(string username, string passwordHash)
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "ValidateUserCredentials";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var paramUsername = command.CreateParameter();
                    paramUsername.ParameterName = "@Username";
                    paramUsername.Value = username;
                    command.Parameters.Add(paramUsername);

                    var paramPassword = command.CreateParameter();
                    paramPassword.ParameterName = "@PasswordHash";
                    paramPassword.Value = passwordHash;
                    command.Parameters.Add(paramPassword);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                RoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                // Role can be loaded separately if needed
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public async Task InsertAuditLogAsync(Guid correlationId, int? userId, string action, string description)
        {
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "InsertAuditLog";
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    var paramCorrelationId = command.CreateParameter();
                    paramCorrelationId.ParameterName = "@CorrelationId";
                    paramCorrelationId.Value = correlationId;
                    command.Parameters.Add(paramCorrelationId);

                    var paramUserId = command.CreateParameter();
                    paramUserId.ParameterName = "@UserId";
                    paramUserId.Value = (object?)userId ?? DBNull.Value;
                    command.Parameters.Add(paramUserId);

                    var paramAction = command.CreateParameter();
                    paramAction.ParameterName = "@Action";
                    paramAction.Value = action;
                    command.Parameters.Add(paramAction);

                    var paramDescription = command.CreateParameter();
                    paramDescription.ParameterName = "@Description";
                    paramDescription.Value = description;
                    command.Parameters.Add(paramDescription);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
