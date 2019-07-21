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
            if (value is Subcommand model)
            {
                Console.WriteLine("Solution:" + model.Solution);
                Console.WriteLine("Name:" + model.Name);
                Console.WriteLine("Framework:" + model.Framework);

                var validators = context.GetServices(typeof(IValidator<New.Subcommand>));

                foreach (IValidator<Subcommand> validator in validators)
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
