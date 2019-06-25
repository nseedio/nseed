using DiffLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Cli.Services
{
    public class DependencyGraphService : IDependencyGraphService
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

        public string GetProjectsPrefix(string solutionPath)
        {
            var dependencyGraph = GenerateDependencyGraph(solutionPath);
            if (dependencyGraph == null)
            {
                return string.Empty;
            }
            var projectNames = dependencyGraph.Projects.Select(p => p.Name).ToList();
            var projectsCommonNamePart = GetCommonValue(projectNames);
            return projectsCommonNamePart.Trim('.');
        }

        public string GetProjectsFramework(string solutionPath)
        {
            var dependencyGraph = GenerateDependencyGraph(solutionPath);
            if (dependencyGraph == null)
            {
                return string.Empty;
            }

            var frameworks = dependencyGraph.Projects
                .SelectMany(p => p.TargetFrameworks, (t, v) =>
                {
                    return v;
                })
                .ToList();


            var frameworkss = dependencyGraph.Projects
                .SelectMany(p => p.TargetFrameworks, (t, v) =>
                {
                    return $"{v.FrameworkName.Framework.ToLower().TrimStart('.')}{v.FrameworkName.Version.Major}.{v.FrameworkName.Version.Minor}";
                })
                .ToList();

            return GetCommonValue(frameworkss);
        }

        private string GetCommonValue(List<string> values)
        {
            if (values == null || !values.Any())
            {
                return string.Empty;
            }

            if (values.Count == 1)
            {
                var value = values.First();
                var valueParts = value.Split(new char[] { '.', '-', '_' });
                return valueParts.FirstOrDefault();
            }

            var diffSection = Diff.CalculateSections(values[0].ToCharArray(), values[1].ToCharArray()).ToList().FirstOrDefault(d => d.IsMatch);
            return values[0].Substring(0, diffSection.LengthInCollection1);
        }
    }
}
