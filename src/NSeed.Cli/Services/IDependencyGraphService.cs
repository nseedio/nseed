using NuGet.ProjectModel;
using System.Collections.Generic;

namespace NSeed.Cli.Services
{
    public interface IDependencyGraphService
    {
        DependencyGraphSpec GenerateDependencyGraph(string projectPath);
        IEnumerable<string> GetSolutionProjectsNames(string solutionPath);
    }
}
