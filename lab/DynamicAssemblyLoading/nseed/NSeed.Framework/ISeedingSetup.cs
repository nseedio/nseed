using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NSeed
{
    public interface ISeedingSetup
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
        IConfiguration BuildConfiguration(string[] commandLineArguments);
    }
}