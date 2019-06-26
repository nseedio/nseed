using NSeed.Cli.Extensions;
using NSeed.Cli.Validation;

namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class NameValidator : Subcommand, IValidator
    {
        public NameValidator()
        {

        }
        public ValidationResult Validate()
        {
            //If name not exist is is empty or null error
            //What if project already exist return change or provide name for project
            //What are special characters that name can posses
            if (Name.IsNotProvidedByUser())
            {
                return ValidationResult.Error("Project name is empty");
            }
            return ValidationResult.Success;
        }
    }
}
