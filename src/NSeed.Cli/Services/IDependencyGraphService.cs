using NuGet.ProjectModel;
using System.Collections.Generic;

namespace NSeed.Cli.Services
{
    internal interface IDependencyGraphService
    {
        DependencyGraphSpec GenerateDependencyGraph(string projectPath);

        IEnumerable<string> GetSolutionProjectsNames(string solutionPath);

        bool ProjectContainsNseedNugetDependency(string projectPath);
    }
}
