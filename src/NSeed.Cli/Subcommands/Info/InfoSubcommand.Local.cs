using McMaster.Extensions.CommandLineUtils;
using NSeed.Cli.Assets;
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

        public string ResolvedProject { get; private set; } = string.Empty;

        public string ResolvedProjectErrorMessage { get; private set; } = string.Empty;

        public void SetResolvedProject(string project)
        {
            ResolvedProject = project;
        }

        public void SetResolvedProjectErrorMessage(string errorMessage)
        {
            ResolvedProjectErrorMessage = errorMessage;
        }

        public async Task OnExecute(
           CommandLineApplication app)
        {
            await Task.Run(() => { });
        }
    }
}
