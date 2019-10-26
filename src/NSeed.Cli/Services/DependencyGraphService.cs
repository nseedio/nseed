using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSeed.Cli.Abstractions;
using NSeed.Cli.Extensions;
using NSeed.Cli.Runners;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NSeed.Cli.Services
{
    internal class DependencyGraphService : IDependencyGraphService
    {
        private IDotNetRunner<DependencyGraphRunnerArgs> DependencyGraphRunner { get; }

        private DependencyGraphSpec dependencyGraphSpec = new DependencyGraphSpec();

        private string projectPath = string.Empty;

        public DependencyGraphService(
            IDotNetRunner<DependencyGraphRunnerArgs> dependencyGraphRunner) => DependencyGraphRunner = dependencyGraphRunner;

        public DependencyGraphSpec GenerateDependencyGraph(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath))
            {
                return new DependencyGraphSpec();
            }

            if (dependencyGraphSpec != null
                && projectPath.Equals(solutionPath, StringComparison.CurrentCultureIgnoreCase))
            {
                return dependencyGraphSpec;
            }

            var args = new DependencyGraphRunnerArgs
            {
                Solution = solutionPath,
                SolutionDirectory = Path.GetDirectoryName(solutionPath),
                OutputPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName())
            };

            var (isSuccessful, message) = DependencyGraphRunner.Run(args);

            if (!isSuccessful)
            {
                dependencyGraphSpec = new DependencyGraphSpec();
                projectPath = string.Empty;
                return dependencyGraphSpec;
            }

            var dependencyGraphText = File.ReadAllText(args.OutputPath);

            dependencyGraphSpec = dependencyGraphText.Exists()
                ? new DependencyGraphSpec(JsonConvert.DeserializeObject<JObject>(dependencyGraphText))
                : new DependencyGraphSpec();

            projectPath = solutionPath;
            return dependencyGraphSpec;
        }

        /// <summary>
        /// Fetching all project names from generated dependency graph of provided solution.
        /// </summary>
        /// <param name="solutionPath">Path to solution (.sln file).</param>
        /// <returns>List of project names.</returns>
        public IEnumerable<string> GetSolutionProjectsNames(string solutionPath)
        {
            var dependencyGraph = GenerateDependencyGraph(solutionPath);
            var projectNames = dependencyGraph.Projects.Select(p => p.Name).ToList();
            return projectNames ?? new List<string>();
        }

        public IOperationResponse<IFramework> GetProjectFramework(string projectPath)
        {
            var dependencyGraph = GenerateDependencyGraph(projectPath);

            var frameworks = dependencyGraph.Projects
                .SelectMany(p => p.TargetFrameworks)
                .Select(f => new Framework(f));

            if (frameworks.Count() > 1)
            {
                return OperationResponse<IFramework>.Error("Multiple frameworks in one project");
            }

            var framework = frameworks.FirstOrDefault();

            if (framework == null)
            {
                return OperationResponse<IFramework>.Error("Framework not found");
            }

            return OperationResponse<IFramework>.Success(framework);
        }

        // public IOperationResponse<Project> GetProject(string projectPath)
        // {
        //    var frameworkResponse = GetProjectFramework(projectPath);
        //    if (!frameworkResponse.IsSuccessful)
        //        return OperationResponse<Project>.Error(frameworkResponse.Message);
        //    var dependencyGraphSpec = GenerateDependencyGraph(projectPath);
        //    var project = dependencyGraphSpec.Projects.FirstOrDefault();
        //    if (project != null)
        //    {
        //        return OperationResponse
        //    }
        //    return OperationResponse.Error("dfvdv");

        // }
    }
}
