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
            IDependencyGraphService dependencyGraphService) => DependencyGraphService = dependencyGraphService;

        public ValidationResult Validate(NewSubcommand command)
        {
            return command.ResolvedName switch
            {
                var name when name.IsNotProvidedByUser() => Error(Resources.New.Errors.ProjectNameNotProvided),

                var name when ContainesUnallowedCharacters(name) ||
                                   ContainesSurrogateCharacters(name) ||
                                   ContainesUnicodeCharacters(name) => Error(Resources.New.Errors.ProjectNameContainsUnallowedCharacters),

                var name when IsReserved(name) => Error(Resources.New.Errors.InvalidProjectName),

                var name when Exist(name, DependencyGraphService.GetSolutionProjectsNames(command.ResolvedSolution)) => Error(Resources.New.Errors.ProjectNameExists),

                var name when name.Length > Resources.New.MaxProjectNameCharacters => Error(Resources.New.Errors.ProjectNameTooLong),

                var name when name.Length < Resources.New.MinProjectNameCharacters => Error(Resources.New.Errors.ProjectNameTooShort),

                _ => Success,
            };
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
