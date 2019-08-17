using NSeed.Cli.Subcommands.New.Models;

namespace NSeed.Cli.Services
{
    internal interface IDotNetRunner
    {
        (bool IsSuccesful, string Message) AddTemplate(string solutionDirectory, Template template);

        (bool IsSuccesful, string Message) CreateProject(string solutionDirectory, string name, string framework, Template template);

        (bool IsSuccesful, string Message) RemoveTemplate(string solutionDirectory, Template template);

        (bool IsSuccesful, string Message) AddProjectToSolution(string solutionDirectory, string solution, string name, Template template);

        RunStatus Run(string workingDirectory, string[] arguments);
    }
}
