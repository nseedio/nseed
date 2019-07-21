using DiffLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Cli.Services
{
    internal class DependencyGraphService : IDependencyGraphService
    {
        private readonly IDotNetRunner dotNetRunner;
        private readonly IFileSystemService fileSystemService;

        private DependencyGraphSpec dependencyGraphSpec = null;
        private string projectPath = string.Empty;

        public DependencyGraphService(
            IDotNetRunner dotNetRunner,
            IFileSystemService fileSystemService)
        {
            this.dotNetRunner = dotNetRunner;
            this.fileSystemService = fileSystemService;
        }

        public DependencyGraphSpec GenerateDependencyGraph(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath))
            {
                return null;
            }

            if (dependencyGraphSpec != null
                && projectPath.Equals(solutionPath, StringComparison.CurrentCultureIgnoreCase))
            {
                return dependencyGraphSpec;
            }

            var dgOutput = fileSystemService.Path.Combine(fileSystemService.Path.GetTempPath(), fileSystemService.Path.GetTempFileName());

            // Use solution path because with that you get dependency graph for all projects from that solutions
            string[] arguments = { "msbuild", $"\"{solutionPath}\"", "/t:GenerateRestoreGraphFile", $"/p:RestoreGraphOutputPath=\"{dgOutput}\"" };

            var runStatus = dotNetRunner.Run(fileSystemService.Path.GetDirectoryName(solutionPath), arguments);

            if (!runStatus.IsSuccess)
            {
                dependencyGraphSpec = null;
                projectPath = string.Empty;
                return null;
            }

            var dependencyGraphText = fileSystemService.File.ReadAllText(dgOutput);
            dependencyGraphSpec = new DependencyGraphSpec(JsonConvert.DeserializeObject<JObject>(dependencyGraphText));
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
    }
}
