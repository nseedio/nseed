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
            return command.ResolvedProject switch
            {
                var project when project.ErrorMessage.Exists() => ValidationResult.Error(command.ResolvedProject.ErrorMessage),
                var project when project.Path.IsNotProvidedByUser() => ValidationResult.Error(Resources.Info.SearchNSeedProjectPathErrors.Instance.WorkingDirectoryDoesNotContainAnyFile),
                _ => ValidationResult.Success
            };
        }
    }
}
