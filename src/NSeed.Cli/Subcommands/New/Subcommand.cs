using McMaster.Extensions.CommandLineUtils;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Subcommands.New.ValueProviders;
using System.Threading.Tasks;


namespace NSeed.Cli.Subcommands.New
{
    using static NSeed.Cli.Resources.Resources;

    [Command("new", Description = New.CommandDescription)]
    [NewValidator]
    internal class Subcommand
    {
        [Option("-s|--solution", Description = New.SolutionDescription)]
        [SolutionDefaultValueProvider]
        public string Solution { get; private set; }

        [Option("-f|--framework", Description = New.FrameworkDescription)]
        [FrameworkDefaultValueProvider]
        public string Framework { get; private set; }

        [Option("-n|--name", Description = New.ProjectNameDescription)]
        [NameDefaultValueProvider(New.DefaultProjectName)]
        public string Name { get; private set; }

        private Task OnExecute(CommandLineApplication app)
        {
            return Task.CompletedTask;
        }
    }
}
