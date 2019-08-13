using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Subcommands.New;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;

namespace NSeed.Cli.Subcommands
{
    internal class DiConfig
    {
        public static void RegisterValidators(IServiceCollection container)
        {
            container
                .AddSingleton<IValidator<NewSubcommand>, SolutionValidator>()
                .AddSingleton<IValidator<NewSubcommand>, NameValidator>()
                .AddSingleton<IValidator<NewSubcommand>, FrameworkValidator>();
        }
    }
}
