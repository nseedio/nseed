using NuGet.ProjectModel;

namespace NSeed.Cli.Services
{
    internal interface IDependencyGraphService
    {
        DependencyGraphSpec GenerateDependencyGraph(string projectPath);
        string GetProjectsPrefix(string solutionPath);
        string GetProjectsFramework(string solutionPath);
    }
}
