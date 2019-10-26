using NSeed.Cli.Abstractions;
using NuGet.ProjectModel;
using System.Collections.Generic;

namespace NSeed.Cli.Services
{
    internal interface IDependencyGraphService
    {
        DependencyGraphSpec GenerateDependencyGraph(string projectPath);

        IEnumerable<string> GetSolutionProjectsNames(string solutionPath);

        IOperationResponse<IFramework> GetProjectFramework(string projectPath);
    }
}
