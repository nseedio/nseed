using Microsoft.EntityFrameworkCore;
using SmokeTests.BusinessModel.Entities;
using SmokeTests.Persistence.Repositories;
using System.Threading.Tasks;

namespace SmokeTests.Persistence.Repositories.EntityFrameworkCore
{
    public class SmokeTestsDbContext : DbContext, IUserRepository, IUnitOfWork
    {
        public DbSet<User> Users { get; protected set; }

        public SmokeTestsDbContext(string connectionString) : base(GetOptions(connectionString))
        {
        }

        public SmokeTestsDbContext(DbContextOptions<SmokeTestsDbContext> options) : base(options)
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            var modelBuilder = new DbContextOptionsBuilder();
            return modelBuilder.UseSqlite(connectionString).Options;
        }

        async Task IUnitOfWork.SaveChangesAsync()
        {
            await SaveChangesAsync();
        }
    }
}