using Microsoft.Extensions.DependencyInjection;

namespace NSeed.Cli.Services
{
    internal class DiConfig
    {
        public static void RegisterServices(IServiceCollection container)
        {
            container
                .AddSingleton<IFileSystemService, FileSystemService>()
                .AddSingleton<IDotNetRunner, DotNetRunner>()
                .AddSingleton<IDependencyGraphService, DependencyGraphService>();
        }
    }
}
