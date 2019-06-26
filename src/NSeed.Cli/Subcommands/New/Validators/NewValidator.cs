using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NSeed.Cli.Subcommands.New.Validators
{

    [AttributeUsage(AttributeTargets.Class)]
    internal class NewValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value is Subcommand model)
            {
                var validators = context.GetServices(typeof(Validation.IValidator))
                    .Where(s=>s.GetType().IsSubclassOf(typeof(Subcommand)));

                foreach (Validation.IValidator validator in validators)
                {
                    var response = validator.Validate();
                    if (!response.IsValid)
                    {
                        return new ValidationResult(response.Message);
                    }
                }
                //You need to validate model
                Console.WriteLine($"Solution: {model.Solution}");
                Console.WriteLine($"Framework: {model.Framework}");
                Console.WriteLine($"Name: {model.Name}");
            }
            return ValidationResult.Success;
        }
    }
}
