using NSeed.Cli.Runners;

namespace NSeed.Cli.Subcommands.New.Runner
{
    internal class NewSubcommandRunner : DotNetRunner, IDotNetRunner<NewSubcommandRunnerArgs>
    {
        public (bool IsSuccessful, string Message) Run(NewSubcommandRunnerArgs args)
        {
            return Run(
                args,
                AddTemplate,
                CreateProject,
                RemoveTemplate,
                AddProjectToSolution);
        }

        private (bool IsSuccesful, string Message) AddTemplate(NewSubcommandRunnerArgs args)
        {
            var arguments = new[] { "new --install", args.Template.Path };
            return Response(Run(args.SolutionDirectory, arguments));
        }

        private (bool IsSuccesful, string Message) CreateProject(NewSubcommandRunnerArgs args)
        {
            var newProjectPath = System.IO.Path.Combine(args.SolutionDirectory, args.Name);
            string[] arguments = new[] { $"new", args.Template.Name, "-n ", args.Name, "-o ", newProjectPath, "-f ", args.Framework };
            return Response(Run(args.SolutionDirectory, arguments));
        }

        private (bool IsSuccesful, string Message) RemoveTemplate(NewSubcommandRunnerArgs args)
        {
            string[] arguments = new[] { "new --uninstall", args.Template.Path };
            return Response(Run(args.SolutionDirectory, arguments));
        }

        private (bool IsSuccesful, string Message) AddProjectToSolution(NewSubcommandRunnerArgs args)
        {
            var newProjectCsprojFilePath = System.IO.Path.Combine(args.SolutionDirectory, args.Name, $"{args.Name}.csproj");
            string[] arguments = new[] { $"sln", args.Solution, "add", newProjectCsprojFilePath };
            return Response(Run(args.SolutionDirectory, arguments));
        }
    }
}
