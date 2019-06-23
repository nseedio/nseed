using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSeed;
using SmokeTests.ServiceConfiguration;

namespace SmokeTests.Seeds
{
    class SeedingSetup : ISeedingSetup
    {
        public IConfiguration BuildConfiguration(string[] commandLineArguments)
        {
            return new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
              .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: false)
              .AddJsonFile("appsettings.Seeds.json", optional: false, reloadOnChange: false)
              .AddEnvironmentVariables()
              .AddCommandLine(commandLineArguments)
              .Build();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ServiceConfigurator.ConfigureServices(services, configuration);
        }
    }
}