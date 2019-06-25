using NuGet.ProjectModel;

namespace NSeed.Cli.Services
{
    public interface IDependencyGraphService
    {
        DependencyGraphSpec GenerateDependencyGraph(string projectPath);
        string GetProjectsPrefix(string solutionPath);
        string GetProjectsFramework(string solutionPath);
    }
}
