using Microsoft.Extensions.DependencyInjection;
using NSeed.Cli.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace NSeed.Cli.Subcommands.Info.Validators
{
    // Todo add generic abstract base validatotr for new and info they are the same just another type
    [AttributeUsage(AttributeTargets.Class)]
    internal class InfoValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is InfoSubcommand model)
            {
                var validators = context.GetServices(typeof(IValidator<InfoSubcommand>));

                foreach (IValidator<InfoSubcommand> validator in validators)
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
