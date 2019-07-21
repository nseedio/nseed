using System.Collections.Generic;
using System.Linq;

namespace NSeed.Cli.Validation
{
    internal class ValidationResult
    {
        public static ValidationResult Success => new ValidationResult { IsValid = true };

        public static ValidationResult Error(string message)
        {
            return new ValidationResult(message) { IsValid = false };
        }

        public ValidationResult()
        {
            Messages = new List<string>();
        }

        public ValidationResult(string errorMessage)
        {
            Messages = new List<string>();
            AddMessage(errorMessage);
        }

        public bool IsValid { get; set; }

        public IList<string> Messages { get; }

        public void AddMessage(string message)
        {
            Messages.Add(message);
        }

        public string Message => Messages?.FirstOrDefault() ?? string.Empty;
    }
}
