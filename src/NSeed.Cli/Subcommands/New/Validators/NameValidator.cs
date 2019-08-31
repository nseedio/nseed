using NSeed.Cli.Assets;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using NSeed.Cli.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using static NSeed.Cli.Validation.ValidationResult;

namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class NameValidator : IValidator<NewSubcommand>
    {
        private IDependencyGraphService DependencyGraphService { get; }

        public NameValidator(
            IDependencyGraphService dependencyGraphService)
        {
            DependencyGraphService = dependencyGraphService;
        }

        public ValidationResult Validate(NewSubcommand command)
        {
            switch (command.ResolvedName)
            {
                case var name when name.IsNotProvidedByUser():
                    return Error(Resources.New.Errors.ProjectNameNotProvided);

                case var name when ContainesUnallowedCharacters(name) ||
                                   ContainesSurrogateCharacters(name) ||
                                   ContainesUnicodeCharacters(name):
                    return Error(Resources.New.Errors.ProjectNameContainsUnallowedCharacters);

                case var name when IsReserved(name):
                    return Error(Resources.New.Errors.InvalidProjectName);

                case var name when Exist(name, DependencyGraphService.GetSolutionProjectsNames(command.ResolvedSolution)):
                    return Error(Resources.New.Errors.ProjectNameExists);

                case var name when name.Length > Resources.New.MaxProjectNameCharacters:
                    return Error(Resources.New.Errors.ProjectNameToLong);

                case var name when name.Length < Resources.New.MinProjectNameCharacters:
                    return Error(Resources.New.Errors.ProjectNameToShort);

                default:
                    return Success;
            }
        }

        private bool Exist(string name, IEnumerable<string> projectNames)
        {
            return projectNames.Any(p => name.Equals(p, StringComparison.OrdinalIgnoreCase));
        }

        // https://stackoverflow.com/questions/773557/which-characters-are-allowed-in-a-vs-project-name
        private bool ContainesUnallowedCharacters(string name)
        {
            var invalidNameCharacters = new char[] { '/', '?', ':', '&', '\\', '*', '"', '<', '>', '|', '#', '%' };
            return name.Any(chr => invalidNameCharacters.Contains(chr));
        }

        private bool ContainesSurrogateCharacters(string name)
        {
            return name.Any(chr => char.IsSurrogate(chr));
        }

        private bool IsReserved(string name)
        {
            var reservedNames = new string[] { "CON", "AUX", "PRN", "COM1", "LPT2", ".", ".." };
            return reservedNames.Any(n => name.Equals(n, StringComparison.OrdinalIgnoreCase));
        }

        private bool ContainesUnicodeCharacters(string name)
        {
            const int maxAnsiCode = 255;

            // Todo: am  Find better way to implement this
            return name.Any(chr => chr > maxAnsiCode);
        }
    }
}
