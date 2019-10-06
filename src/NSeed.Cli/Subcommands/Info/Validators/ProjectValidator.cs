using NSeed.Cli.Assets;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using NSeed.Cli.Validation;

namespace NSeed.Cli.Subcommands.Info.Validators
{
    internal class ProjectValidator : IValidator<InfoSubcommand>
    {
        public IFileSystemService FileSystemService { get; }

        public IDependencyGraphService DependencyGraphService { get; }

        public ProjectValidator(
            IFileSystemService fileSystemService,
            IDependencyGraphService dependencyGraphService)
        {
            FileSystemService = fileSystemService;
            DependencyGraphService = dependencyGraphService;
        }

        public ValidationResult Validate(InfoSubcommand command)
        {
            if (command.ResolvedProject.IsNotProvidedByUser())
            {
                return command.ResolvedProjectErrorMessage.Exists() ?
                    ValidationResult.Error(command.ResolvedProjectErrorMessage) :
                    ValidationResult.Error(Resources.Info.SearchNSeedProjectPathErrors.Instance.WorkingDirectoryDoesNotContainAnyFile);
            }

            var response = FileSystemService.GetNSeedProjectPath(command.ResolvedProject);

            if (!response.IsSuccessful)
            {
                return ValidationResult.Error(response.Message);
            }

            if (!DependencyGraphService.ProjectContainsNseedNugetDependency(command.ResolvedProject))
            {
                return ValidationResult.Error("Provided project is invalid NSeed dependency is not found");
            }

            return ValidationResult.Success;
        }
    }
}
