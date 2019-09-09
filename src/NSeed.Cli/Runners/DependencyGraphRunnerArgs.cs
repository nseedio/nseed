namespace NSeed.Cli.Runners
{
    internal class DependencyGraphRunnerArgs
    {
        public string SolutionDirectory { get; set; } = string.Empty;

        public string Solution { get; set; } = string.Empty;

        public string OutputPath { get; set; } = string.Empty;
    }
}
