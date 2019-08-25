using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace NSeed.Cli.Subcommands.New.Validators
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class NewValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is NewSubcommand model)
            {
                var validators = context.GetServices(typeof(IValidator<NewSubcommand>));

                foreach (IValidator<NewSubcommand> validator in validators)
                {
                    var response = validator.Validate(model);
                    if (!response.IsValid)
                    {
                        return new ValidationResult(response.Message);
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
