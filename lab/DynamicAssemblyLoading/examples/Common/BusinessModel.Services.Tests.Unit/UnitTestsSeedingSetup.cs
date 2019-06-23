using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSeed;
using SmokeTests.ServiceConfiguration;

#if DotNetCore
using SmokeTests.Persistence.Repositories.EntityFrameworkCore;
#endif

namespace BusinessModel.Services.Tests.Unit
{
    class UnitTestsSeedingSetup : ISeedingSetup
    {
        private readonly SmokeTestsDbContext dbContext;

        public UnitTestsSeedingSetup(SmokeTestsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IConfiguration BuildConfiguration(string[] commandLineArguments)
        {
            return new ConfigurationBuilder()
                .Build();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ServiceConfigurator.ConfigureServices(services, configuration);

            services.AddScoped(_ => dbContext);
        }
    }
}
