using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSeed;
using SmokeTests.ServiceConfiguration;

namespace BusinessModel.Services.Tests.Integration
{
    class IntegrationTestsSeedingSetup : ISeedingSetup
    {
        public IConfiguration BuildConfiguration(string[] commandLineArguments)
        {
            return new ConfigurationBuilder()
                .Build();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ServiceConfigurator.ConfigureServices(services, configuration);
        }
    }
}
