using NSeed.Cli.Abstractions;
using NuGet.ProjectModel;
using System.Collections.Generic;

namespace NSeed.Cli.Services
{
    internal interface IDependencyGraphService
    {
        DependencyGraphSpec GenerateDependencyGraph(string projectPath, bool useCache = true);

        IEnumerable<string> GetSolutionProjectsNames(string solutionPath, bool useCache = true);

        IOperationResponse<IFramework> GetProjectFramework(string projectPath);
    }
}
