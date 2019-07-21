using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;

namespace NSeed.Cli.Subcommands
{
    internal class DiConfig
    {
        public static void RegisterValidators(IServiceCollection container)
        {
            container
                .AddSingleton<IValidator<New.Subcommand>, SolutionValidator>()
                .AddSingleton<IValidator<New.Subcommand>, NameValidator>()
                .AddSingleton<IValidator<New.Subcommand>, FrameworkValidator>();
        }
    }
}
