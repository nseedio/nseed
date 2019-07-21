using NuGet.ProjectModel;
using System.Collections.Generic;

namespace NSeed.Cli.Services
{
    /// <summary>
    /// Service that create solution dependency graph.
    /// </summary>
    public interface IDependencyGraphService
    {
        DependencyGraphSpec GenerateDependencyGraph(string projectPath);

        IEnumerable<string> GetSolutionProjectsNames(string solutionPath);
    }
}
