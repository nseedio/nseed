using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
using NSeed.Cli.Runners;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.Info;
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

        public static IServiceCollection AddSeedBucketVerifier(this IServiceCollection services)
        {
            return services
               .AddSingleton<Func<FrameworkType, ISeedBucketVerifier>>(serviceProvider => key =>
               {
                   var dependencyGraphService = serviceProvider.GetRequiredService<IDependencyGraphService>();
                   var fileSystemService = serviceProvider.GetRequiredService<IFileSystemService>();

                   return key switch
                   {
                       FrameworkType.NETCoreApp => new SeedBucketVerifier(dependencyGraphService, fileSystemService, serviceProvider.GetRequiredService<CoreNugetPackageDetector>()),
                       FrameworkType.NETFramework => new SeedBucketVerifier(dependencyGraphService, fileSystemService, serviceProvider.GetRequiredService<ClassicNugetPackageDetector>()),
                       _ => throw new MissingMemberException()
                   };
               });
        }
    }
}
