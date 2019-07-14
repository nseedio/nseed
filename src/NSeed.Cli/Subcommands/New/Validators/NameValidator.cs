using NSeed.Cli.Extensions;
using NSeed.Cli.Validation;
using System;

namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class NameValidator : IValidator<New.Subcommand>
    {
        public NameValidator()
        {

        }
        
        public ValidationResult Validate(Subcommand command)
        {
            //If name not exist is is empty or null error
            //What if project already exist return change or provide name for project
            //What are special characters that name can posses
            if (command.ResolvedName.IsNotProvidedByUser())
            {
                return ValidationResult.Error("Project name is empty");
            }
            return ValidationResult.Success;
        }
    }
}
