namespace NSeed.Cli.Abstractions
{
    internal interface IDetector
    {
        public IOperationResponse<Project> Detect(Project project);
    }
}
