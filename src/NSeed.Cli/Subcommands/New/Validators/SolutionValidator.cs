using NSeed.Cli.Assets;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using NSeed.Cli.Validation;

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
            if (command.IsValidResolvedSolution)
            {
                return ValidationResult.Success;
            }

            if (command.ResolvedSolution.IsNotProvidedByUser())
            {
                return command.ResolvedSolutionErrorMessage.Exists() ?
                    ValidationResult.Error(command.ResolvedSolutionErrorMessage) :
                    ValidationResult.Error(Resources.New.SearchSolutionPathErrors.Instance.WorkingDirectoryDoesNotContainAnyFile);
            }

            var response = FileSystemService.GetSolutionPath(command.ResolvedSolution);

            if (!response.IsSuccessful)
            {
                return ValidationResult.Error(response.Message);
            }

            var dependencyGraph = DependencyGraphService.GenerateDependencyGraph(response?.Payload ?? string.Empty);

            if (dependencyGraph == null)
            {
                return ValidationResult.Error(Resources.New.Errors.InvalidSolution);
            }

            command.ResolvedSolutionIsValid();
            return ValidationResult.Success;
        }
    }
}
