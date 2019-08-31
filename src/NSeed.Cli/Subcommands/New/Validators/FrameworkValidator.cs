using NSeed.Cli.Assets;
using NSeed.Cli.Extensions;
using NSeed.Cli.Validation;
using System;
using System.Linq;
using static NSeed.Cli.Assets.Resources;

namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class FrameworkValidator : IValidator<NewSubcommand>
    {
        public ValidationResult Validate(NewSubcommand command)
        {
            if (command.ResolvedFramework.IsNotProvidedByUser())
            {
                return ValidationResult.Error(Resources.New.Errors.FrameworkNotProvided);
            }

            switch (command.ResolvedFrameworkWithVersion)
            {
                case var framework when framework.Name is Framework.None:
                    return ValidationResult.Error(Resources.New.Errors.InvalidFramework);

                case var framework when framework.Name is Framework.NETCoreApp
                                        && !DotNetCoreVersions.ToList().Contains(framework.Version):
                    return ValidationResult.Error(Resources.New.Errors.InvalidDotNetCoreVersion);

                case var framework when framework.Name is Framework.NETFramework
                                        && !DotNetClassicVersions.ToList().Contains(framework.Version):
                    return ValidationResult.Error(Resources.New.Errors.InvalidDotNetClassicVersion);

                default:
                    break;
            }

            return ValidationResult.Success;
        }
    }
}
