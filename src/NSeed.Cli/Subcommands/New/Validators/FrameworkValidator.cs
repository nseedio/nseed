using NSeed.Cli.Extensions;
using NSeed.Cli.Validation;

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
            return ValidationResult.Success;
        }
    }
}
