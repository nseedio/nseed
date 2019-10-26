using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
using NSeed.Cli.Runners;
using NSeed.Cli.Subcommands.Info;
using NSeed.Cli.Subcommands.Info.Detector;
using NSeed.Cli.Subcommands.Info.Runner;
using NSeed.Cli.Subcommands.Info.Validators;
using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Subcommands.New.Runner;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;
using System;

namespace NSeed.Cli.Subcommands
{
    internal static class DiConfig
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services
                .AddSingleton<IValidator<NewSubcommand>, SolutionValidator>()
                .AddSingleton<IValidator<NewSubcommand>, NameValidator>()
                .AddSingleton<IValidator<NewSubcommand>, FrameworkValidator>()
                .AddSingleton<IValidator<InfoSubcommand>, ProjectValidator>();
        }

        public static IServiceCollection AddSubcommandRunners(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDotNetRunner<NewSubcommandRunnerArgs>, NewSubcommandRunner>()
                .AddSingleton<InfoSubcommandCoreRunner>()
                .AddSingleton<InfoSubcommandClassicRunner>()
                .AddSingleton<Func<FrameworkType, IDotNetRunner<InfoSubcommandRunnerArgs>>>(serviceProvider => key =>
                {
                    return key switch
                    {
                        FrameworkType.NETCoreApp => serviceProvider.GetRequiredService<InfoSubcommandCoreRunner>(),
                        FrameworkType.NETFramework => serviceProvider.GetRequiredService<InfoSubcommandClassicRunner>(),
                        _ => throw new MissingMemberException()
                    };
                });
        }

        public static IServiceCollection AddDetectors(this IServiceCollection services)
        {
            return services
               .AddSingleton<NSeedClassicDetector>()
               .AddSingleton<NSeedCoreDetector>()
               .AddSingleton<Func<FrameworkType, IDetector>>(serviceProvider => key =>
               {
                   return key switch
                   {
                       FrameworkType.NETCoreApp => serviceProvider.GetRequiredService<NSeedCoreDetector>(),
                       FrameworkType.NETFramework => serviceProvider.GetRequiredService<NSeedClassicDetector>(),
                       _ => serviceProvider.GetRequiredService<NSeedCoreDetector>(),
                   };
               });
        }
    }
}
