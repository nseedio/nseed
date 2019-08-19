using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
