using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using NSeed.Cli.Validation;

namespace NSeed.Cli.Subcommands.New.Validators
{
    internal class SolutionValidator : Subcommand, IValidator
    {
        public IFileSystemService FileSystemService { get; }
        public IDependencyGraphService DependencyGraphService { get; }

        public SolutionValidator(IFileSystemService fileSystemService,
            IDependencyGraphService dependencyGraphService)
        {
            FileSystemService = fileSystemService;
            DependencyGraphService = dependencyGraphService;
        }

        public ValidationResult Validate()
        {
            if (Solution.IsNotProvidedByUser())
            {
                return ValidationResult.Error("Solution is empty"); 
            }
            return ValidationResult.Success;
        }
    }
}
