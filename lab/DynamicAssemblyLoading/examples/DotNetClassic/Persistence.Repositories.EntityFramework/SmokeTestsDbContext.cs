using SmokeTests.BusinessModel.Entities;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SmokeTests.Persistence.Repositories.EntityFramework
{
    public class SmokeTestsDbContext : DbContext, IUserRepository, IUnitOfWork
    {
        public DbSet<User> Users { get; set; }

        public SmokeTestsDbContext() : base("SmokeTestsConnectionString")
        {
        }

        public SmokeTestsDbContext(string connectionString) : base(connectionString)
        {
        }

        async Task IUnitOfWork.SaveChangesAsync()
        {
            await SaveChangesAsync();
        }
    }
}