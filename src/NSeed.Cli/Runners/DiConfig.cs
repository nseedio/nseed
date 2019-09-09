using Microsoft.Extensions.DependencyInjection;

namespace NSeed.Cli.Runners
{
    internal static class DiConfig
    {
        public static IServiceCollection AddRunners(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDotNetRunner<DependencyGraphRunnerArgs>, DependencyGraphRunner>();
        }
    }
}
