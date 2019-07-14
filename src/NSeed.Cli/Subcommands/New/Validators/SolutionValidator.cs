using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using NSeed.Cli.Validation;
using System;
using static NSeed.Cli.Resources.Resources;


namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class SolutionValidator : IValidator<New.Subcommand>
    {
        public IFileSystemService FileSystemService { get; }
        public IDependencyGraphService DependencyGraphService { get; }

        public SolutionValidator(IFileSystemService fileSystemService,
            IDependencyGraphService dependencyGraphService)
        {
            FileSystemService = fileSystemService;
            DependencyGraphService = dependencyGraphService;
        }

        public ValidationResult Validate(Subcommand command)
        {
            if (command.ResolvedSolutionIsValid)
            {
                return ValidationResult.Success;
            }

            if (command.ResolvedSoluiton.IsNotProvidedByUser())
            {
                return ValidationResult.Error(Error.SolutionPathIsNotProvided);
            }

            var (IsSuccesful, Message) = FileSystemService.TryGetSolutionPath(command.ResolvedSoluiton, out var path);
            if (!IsSuccesful)
            {
                return ValidationResult.Error(Message);
            }

            var dependencyGraph = DependencyGraphService.GenerateDependencyGraph(path);
            if (dependencyGraph == null)
            {
                return ValidationResult.Error("Provided solution is invalid Solution can't be processed dependencyGraph");
            }
            command.ResolvedSolutionIsValid = true;
            return ValidationResult.Success;
        }
    }
}
