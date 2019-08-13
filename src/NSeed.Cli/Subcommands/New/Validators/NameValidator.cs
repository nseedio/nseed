using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using NSeed.Cli.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using static NSeed.Cli.Validation.ValidationResult;

namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class NameValidator : IValidator<New.Subcommand>
    {
        private IDependencyGraphService DependencyGraphService { get; }

        private const int MinCharactersSize = 3;
        private const int MaxCharactersSize = 50;

        public NameValidator(
            IDependencyGraphService dependencyGraphService)
        {
            DependencyGraphService = dependencyGraphService;
        }

        public ValidationResult Validate(Subcommand command)
        {
            switch (command.ResolvedName)
            {
                case var name when name.IsNotProvidedByUser():
                    return Error("Project name is empty");

                case var name when ContainesUnallowedCharacters(name) ||
                                   ContainesSurrogateCharacters(name) ||
                                   ContainesUnicodeCharacters(name):
                    return Error("Project name contain unallowed characters");

                case var name when IsReserved(name):
                    return Error("Project name is invalid");

                case var name when Exist(name, DependencyGraphService.GetSolutionProjectsNames(command.ResolvedSolution)):
                    return Error("Project name already exist");

                case var name when name.Length > MaxCharactersSize:
                    return Error($"Project name is to long Max. {MaxCharactersSize} characters");

                case var name when name.Length < MinCharactersSize:
                    return Error($"Project name is to short Min. {MinCharactersSize} characters");

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

            // TODO:am  Find better way to implement this
            return name.Any(chr => chr > maxAnsiCode);
        }
    }
}
