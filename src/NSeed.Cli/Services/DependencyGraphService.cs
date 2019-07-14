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
        private readonly IDotNetRunner DotNetRunner;
        private readonly IFileSystemService FileSystemService;

        private DependencyGraphSpec DependencyGraphSpec = null;
        private string ProjectPath = string.Empty;

        public DependencyGraphService(IDotNetRunner dotNetRunner,
            IFileSystemService fileSystemService)
        {
            DotNetRunner = dotNetRunner;
            FileSystemService = fileSystemService;
        }

        public DependencyGraphSpec GenerateDependencyGraph(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath))
            {
                return null;
            }

            if (DependencyGraphSpec != null
                && ProjectPath.Equals(solutionPath, StringComparison.CurrentCultureIgnoreCase))
            {
                return DependencyGraphSpec;
            }

            var dgOutput = FileSystemService.Path.Combine(FileSystemService.Path.GetTempPath(), FileSystemService.Path.GetTempFileName());

            //Use solution path because with that you get dependency graph for all projects from that solutions
            string[] arguments = { "msbuild", $"\"{solutionPath}\"", "/t:GenerateRestoreGraphFile", $"/p:RestoreGraphOutputPath=\"{dgOutput}\"" };

            var runStatus = DotNetRunner.Run(FileSystemService.Path.GetDirectoryName(solutionPath), arguments);

            if (!runStatus.IsSuccess)
            {
                DependencyGraphSpec = null;
                ProjectPath = string.Empty;
                return null;
            }

            var dependencyGraphText = FileSystemService.File.ReadAllText(dgOutput);
            DependencyGraphSpec = new DependencyGraphSpec(JsonConvert.DeserializeObject<JObject>(dependencyGraphText));
            ProjectPath = solutionPath;
            return DependencyGraphSpec;
        }

        /// <summary>
        /// Fetching all project names from generated dependency graph of provided solution
        /// </summary>
        /// <param name="solutionPath"></param>
        /// <returns>List of project names</returns>
        public IEnumerable<string> GetSolutionProjectsNames(string solutionPath)
        {
            var dependencyGraph = GenerateDependencyGraph(solutionPath);
            var projectNames = dependencyGraph.Projects.Select(p => p.Name).ToList();
            return projectNames ?? new List<string>();
        }
    }
}
