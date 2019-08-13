using NSeed.Cli.Extensions;
using NSeed.Cli.Validation;
using System;
using System.Linq;
using static NSeed.Cli.Assets.Resources;

namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class FrameworkValidator : IValidator<NewSubcommand>
    {
        public FrameworkValidator()
        {
        }

        public ValidationResult Validate(NewSubcommand command)
        {
            if (command.ResolvedFramework.IsNotProvidedByUser())
            {
                return ValidationResult.Error("Project framework is empty");
            }

            var framework = command.GetFrameworkWithVersion();
            switch (framework.Name)
            {
                case Assets.Framework.NETCoreApp:
                    if (!DotNetCoreVersions.Any(v => framework.Version.Equals(v, StringComparison.OrdinalIgnoreCase)))
                    {
                        return ValidationResult.Error("Core project framework version is not valid");
                    }

                    break;
                case Assets.Framework.NETFramework:
                    if (!DotNetClassicVersions.Any(v => framework.Version.Equals(v, StringComparison.OrdinalIgnoreCase)))
                    {
                        return ValidationResult.Error("Full dotnet project version is not valid");
                    }

                    break;
                case Assets.Framework.None:
                    return ValidationResult.Error("Project framework is invalid");
            }

            return ValidationResult.Success;
        }
    }
}
