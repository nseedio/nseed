using Microsoft.Extensions.DependencyInjection;

namespace NSeed.Cli.Services
{
    internal static class DiConfig
    {
        public static IServiceCollection AddCliServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IFileSystemService, FileSystemService>()
                .AddSingleton<IDependencyGraphService, DependencyGraphService>();
        }
    }
}
