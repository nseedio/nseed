using McMaster.Extensions.CommandLineUtils;
using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
using NSeed.Cli.Runners;
using NSeed.Cli.Subcommands.Info.Runner;
using NSeed.Cli.Subcommands.Info.Validators;
using NSeed.Cli.Subcommands.Info.ValueProviders;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands.Info
{
    [InfoValidator]
    internal partial class InfoSubcommand
    {
        [Option("-p|--project", Description = Resources.Info.ProjectDescription)]
        [ProjectDefaultValueProvider]
        public string Project { get; private set; } = string.Empty;

        public Project ResolvedProject { get; private set; } = new Project();

        public string ResolvedProjectErrorMessage { get; private set; } = string.Empty;

        public void SetResolvedProject(Project project)
        {
            ResolvedProject = project;
        }

        public void SetResolvedProjectErrorMessage(string errorMessage)
        {
            ResolvedProjectErrorMessage = errorMessage;
        }

        public async Task OnExecute(
           CommandLineApplication app,
           IDotNetRunner<InfoSubcommandRunnerArgs> dotNetRunner)
        {
            dotNetRunner.Run(new InfoSubcommandRunnerArgs
            {
                NseedProject = ResolvedProject.Path,
                NseedProjectDirectory = ResolvedProject.Directory
            });
            await Task.Run(() => { });
        }
    }
}
