using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;

namespace NSeed.Cli.Subcommands
{
    internal static class DiConfig
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services
                .AddSingleton<IValidator<NewSubcommand>, SolutionValidator>()
                .AddSingleton<IValidator<NewSubcommand>, NameValidator>()
                .AddSingleton<IValidator<NewSubcommand>, FrameworkValidator>();
        }
    }
}
