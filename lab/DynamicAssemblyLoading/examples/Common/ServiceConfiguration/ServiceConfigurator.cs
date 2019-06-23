using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmokeTests.BusinessModel.Services.Users;
using SmokeTests.Persistence.Repositories;
using SmokeTests.Persistence.Repositories.EntityFrameworkCore;

namespace SmokeTests.ServiceConfiguration
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddScoped(_ => new SmokeTestsDbContext(configuration.GetConnectionString("SmokeTestsConnectionString")));

            // SmokeTestsDbContext class implements the repositories
            // and the IUnitOfWork. This is the only way to get the same
            // instance as the implementation of several services.
            // See: https://stackoverflow.com/questions/41810986/asp-net-core-register-implementation-with-multiple-interfaces-and-lifestyle-sin/41812930
            services.AddScoped(serviceProvider => (IUserRepository)serviceProvider.GetRequiredService<SmokeTestsDbContext>());
            services.AddScoped(serviceProvider => (IUnitOfWork)serviceProvider.GetRequiredService<SmokeTestsDbContext>());

            services
                .AddScoped<IUserService, UserService>();
        }
    }
}