using Microsoft.EntityFrameworkCore;
using CareLite.Models.Domain;

namespace CareLite.Data
{
    public class CareLiteDbContext : DbContext
    {
        public CareLiteDbContext(DbContextOptions<CareLiteDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
    }
}