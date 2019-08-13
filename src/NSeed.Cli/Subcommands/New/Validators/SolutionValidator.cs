using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using NSeed.Cli.Validation;
using static NSeed.Cli.Resources.Resources;

namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class SolutionValidator : IValidator<NewSubcommand>
    {
        public IFileSystemService FileSystemService { get; }

        public IDependencyGraphService DependencyGraphService { get; }

        public SolutionValidator(
            IFileSystemService fileSystemService,
            IDependencyGraphService dependencyGraphService)
        {
            FileSystemService = fileSystemService;
            DependencyGraphService = dependencyGraphService;
        }

        public ValidationResult Validate(NewSubcommand command)
        {
            if (command.ResolvedSolutionIsValid)
            {
                return ValidationResult.Success;
            }

            if (command.ResolvedSolution.IsNotProvidedByUser())
            {
                return ValidationResult.Error(Error.SolutionPathIsNotProvided);
            }

            var (isSuccesful, message) = FileSystemService.TryGetSolutionPath(command.ResolvedSolution, out var path);
            if (!isSuccesful)
            {
                return ValidationResult.Error(message);
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
