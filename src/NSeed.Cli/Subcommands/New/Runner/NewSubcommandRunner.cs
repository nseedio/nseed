using NSeed.Cli.Runners;
using NSeed.Cli.Services;
using System.Linq;

namespace NSeed.Cli.Subcommands.New.Runner
{
    internal class NewSubcommandRunner : DotNetRunner, IDotNetRunner<NewSubcommandRunnerArgs>
    {
        private IDependencyGraphService DependencyGraphService { get; }

        public NewSubcommandRunner(IDependencyGraphService dependencyGraphService) => DependencyGraphService = dependencyGraphService;

        public (bool IsSuccessful, string Message) Run(NewSubcommandRunnerArgs args)
        {
            return Run(
                args,
                AddTemplate,
                CreateProject,
                RemoveTemplate,
                AddProjectToSolution,
                DoesSolutionContainsProject);
        }

        private (bool IsSuccesful, string Message) AddTemplate(NewSubcommandRunnerArgs args)
        {
            var arguments = new[] { "new --install", args.Template.DirectoryName };
            return Response(RunDotNet(args.SolutionDirectory, arguments));
        }

        private (bool IsSuccesful, string Message) CreateProject(NewSubcommandRunnerArgs args)
        {
            var newProjectPath = System.IO.Path.Combine(args.SolutionDirectory, args.Name);
            string[] arguments = new[] { $"new", args.Template.Name, "-n ", args.Name, "-o ", newProjectPath, "-f ", args.Framework };
            return Response(RunDotNet(args.SolutionDirectory, arguments));
        }

        private (bool IsSuccesful, string Message) RemoveTemplate(NewSubcommandRunnerArgs args)
        {
            string[] arguments = new[] { "new --uninstall", args.Template.DirectoryName };
            return Response(RunDotNet(args.SolutionDirectory, arguments));
        }

        private (bool IsSuccesful, string Message) AddProjectToSolution(NewSubcommandRunnerArgs args)
        {
            var newProjectCsprojFilePath = System.IO.Path.Combine(args.SolutionDirectory, args.Name, $"{args.Name}.csproj");
            string[] arguments = new[] { $"sln", args.Solution, "add", newProjectCsprojFilePath };
            return Response(RunDotNet(args.SolutionDirectory, arguments));
        }

        private (bool IsSuccesful, string Message) DoesSolutionContainsProject(NewSubcommandRunnerArgs args)
        {
            var solutionProjectNames = DependencyGraphService.GetSolutionProjectsNames(args.Solution, useCache: false);
            return solutionProjectNames.Any(name => name.Equals(args.Name))
                ? (true, string.Empty)
                : (false, Assets.Resources.New.Errors.ProjectNotAddedToSolution);
        }
    }
}
