using NSeed.Cli.Extensions;
using NSeed.Cli.Validation;
using System;
using System.Linq;
using static NSeed.Cli.Resources.Resources;

namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class FrameworkValidator : IValidator<New.Subcommand>
    {
        public FrameworkValidator()
        {
        }

        public ValidationResult Validate(Subcommand command)
        {
            if (command.ResolvedFramework.IsNotProvidedByUser())
            {
                return ValidationResult.Error("Project framework is empty");
            }

            var bla = command.GetFrameworkWithVersion();
            switch (bla.Name)
            {
                case Resources.Framework.NETCoreApp:
                    if (!DotNetCoreVersions.Any(v => bla.Version.Equals(v, StringComparison.OrdinalIgnoreCase)))
                    {
                        return ValidationResult.Error("Core project framework version is not valid");
                    }

                    break;
                case Resources.Framework.NETFramework:
                    if (!FullDotNetVersions.Any(v => bla.Version.Equals(v, StringComparison.OrdinalIgnoreCase)))
                    {
                        return ValidationResult.Error("Full dotnet project version is not valid");
                    }

                    break;
            }

            return ValidationResult.Success;
        }
    }
}
