using NSeed.Cli.Extensions;
using NSeed.Cli.Validation;
using System;

namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class FrameworkValidator : Subcommand, IValidator
    {
        public FrameworkValidator()
        {

        }
        public ValidationResult Validate()
        {
            if (Framework.IsNotProvidedByUser())
            {
                return ValidationResult.Error("Framework is empty");
            }

            return ValidationResult.Success;
        }
    }
}
