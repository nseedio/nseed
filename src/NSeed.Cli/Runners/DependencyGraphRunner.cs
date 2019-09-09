namespace NSeed.Cli.Runners
{
    internal class DependencyGraphRunner : DotNetRunner, IDotNetRunner<DependencyGraphRunnerArgs>
    {
        public (bool IsSuccessful, string Message) Run(DependencyGraphRunnerArgs args)
        {
            return GenerateDependencyGraph(args);
        }

        private (bool IsSuccesful, string Message) GenerateDependencyGraph(DependencyGraphRunnerArgs args)
        {
            string[] arguments = { "msbuild", $"\"{args.Solution}\"", "/t:GenerateRestoreGraphFile", $"/p:RestoreGraphOutputPath=\"{args.OutputPath}\"" };
            return Response(Run(args.SolutionDirectory, arguments));
        }
    }
}
