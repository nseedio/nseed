using Microsoft.EntityFrameworkCore.Design;

namespace SmokeTests.Persistence.Repositories.EntityFrameworkCore
{
    public class SmokeTestsDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SmokeTestsDbContext>
    {
        public SmokeTestsDbContext CreateDbContext(string[] args)
        {
            return new SmokeTestsDbContext("Data Source=C:\\Temp\\SmokeTests.db");
        }
    }
}