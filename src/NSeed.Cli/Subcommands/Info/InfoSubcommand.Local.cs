using McMaster.Extensions.CommandLineUtils;
using NSeed.Abstractions;
using NSeed.Cli.Abstractions;
using NSeed.Cli.Assets;
using NSeed.Cli.Runners;
using NSeed.Cli.Subcommands.Info.Runner;
using NSeed.Cli.Subcommands.Info.Validators;
using NSeed.Cli.Subcommands.Info.ValueProviders;
using System;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands.Info
{
    [InfoValidator]
    internal partial class InfoSubcommand
    {
        private readonly IOutputSink output;

        [Option("-p|--project", Description = Resources.Info.ProjectDescription)]
        [ProjectDefaultValueProvider]
        public string Project { get; private set; } = string.Empty;

        public Project ResolvedProject { get; private set; } = Abstractions.Project.Empty;

        public void SetResolvedProject(Project project)
        {
            ResolvedProject = project;
        }

        public InfoSubcommand(IOutputSink output)
        {
            this.output = output;
        }

        public async Task OnExecute(Func<FrameworkType, IDotNetRunner<InfoSubcommandRunnerArgs>> runnerResolverFunc)
        {
            var runner = runnerResolverFunc(ResolvedProject.Framework.Type);
            var result = runner.Run(new InfoSubcommandRunnerArgs
            {
                NSeedProjectPath = ResolvedProject.Path,
                NSeedProjectDirectory = ResolvedProject.Directory,
                NSeedProjectName = ResolvedProject.Name
            });

            if (!result.IsSuccessful)
            {
                output.WriteError(result.Message);
            }

            await Task.Run(() => { });
        }
    }
}
