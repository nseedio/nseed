namespace NSeed.Cli.Services
{
    internal interface IDotNetRunner
    {
        RunStatus Run(string workingDirectory, string[] arguments);
    }
}
