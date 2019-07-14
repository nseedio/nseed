using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands
{
    internal class DiConfig
    {
        public static void RegisterValidators(IServiceCollection container)
        {
            container
                .AddSingleton<IValidator<New.Subcommand>, SolutionValidator>()
                .AddSingleton<IValidator<New.Subcommand>, NameValidator>()
                .AddSingleton<IValidator<New.Subcommand>, FrameworkValidator >();
        }
    }
}
