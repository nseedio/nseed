using NSeed.Cli.Assets;
using NSeed.Cli.Validation;
using System.Linq;
using static NSeed.Cli.Assets.Resources;

namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class FrameworkValidator : IValidator<NewSubcommand>
    {
        public ValidationResult Validate(NewSubcommand command)
        {
            return command.ResolvedFramework switch
            {
                var framework when framework.Type is FrameworkType.None =>
                        ValidationResult.Error(Resources.New.Errors.FrameworkNotProvided),

                var framework when framework.Type is FrameworkType.Undefined =>
                        ValidationResult.Error(Resources.New.Errors.InvalidFramework),

                var framework when framework.Type is FrameworkType.NETCoreApp
                                        && !DotNetCoreVersions.Contains(framework.Version) =>
                        ValidationResult.Error(Resources.New.Errors.InvalidDotNetCoreVersion),

                var framework when framework.Type is FrameworkType.NETFramework
                                        && !DotNetClassicVersions.Contains(framework.Version) =>
                        ValidationResult.Error(Resources.New.Errors.InvalidDotNetClassicVersion),

                _ => ValidationResult.Success,
            };
        }
    }
}
