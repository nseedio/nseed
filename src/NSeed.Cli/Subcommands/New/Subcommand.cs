using DiffLib;
using McMaster.Extensions.CommandLineUtils;
using NSeed.Cli.Extensions;
using NSeed.Cli.Services;
using NSeed.Cli.Subcommands.New.Validators;
using NSeed.Cli.Subcommands.New.ValueProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands.New
{
    using static NSeed.Cli.Resources.Resources;

    [Command("new", Description = New.CommandDescription)]
    [NewValidator]
    internal class Subcommand
    {
        [Option("-s|--solution", Description = New.SolutionDescription)]
        [SolutionDefaultValueProvider]
        public string Solution { get; private set; }

        [Option("-f|--framework", Description = New.FrameworkDescription)]
        [FrameworkDefaultValueProvider]
        public string Framework { get; private set; }

        [Option("-n|--name", Description = New.ProjectNameDescription)]
        [NameDefaultValueProvider(New.DefaultProjectName)]
        public string Name { get; private set; }

        public string ResolvedSoluiton { get; private set; } = string.Empty;
        public string ResolvedFramework { get; private set; } = string.Empty;
        public string ResolvedName { get; private set; } = string.Empty;
        public bool ResolvedSolutionIsValid { get; set; } = false;

        public void SetResolvedSolution(string solution)
        {
            ResolvedSoluiton = solution;
        }
        public void SetResolvedName(string name)
        {
            ResolvedName = name;
        }
        public void SetResolvedFramework(string framework)
        {
            ResolvedFramework = framework;
        }

        public void ResolveDefaultNameWithPrefix(IDependencyGraphService dependencyGraphService, string defaultName)
        {
            if (ResolvedSolutionIsValid && dependencyGraphService != null)
            {
                SetResolvedName(defaultName);
                var projectNames = dependencyGraphService.GetSolutionProjectsNames(ResolvedSoluiton).ToList();
                var commonPrefix = GetCommonValue(projectNames);
                if (commonPrefix.Exists())
                {
                    SetResolvedName($"{commonPrefix}.{defaultName}");
                }
            }
        }
        public void ResolveFramework(IDependencyGraphService dependencyGraphService)
        {
            if (ResolvedSolutionIsValid && dependencyGraphService != null)
            {
                var dependencyGraph = dependencyGraphService.GenerateDependencyGraph(ResolvedSoluiton);
                if (dependencyGraph != null)
                {
                    var frameworks = dependencyGraph.Projects
                        .SelectMany(p => p.TargetFrameworks, (t, v) => { return v; })
                        .ToList();

                    if (frameworks.Any())
                    {
                        var frameworkNames = frameworks.Select(f => $"{f.FrameworkName.Framework}{f.FrameworkName.Version.Major}{f.FrameworkName.Version.Minor}{f.FrameworkName.Version.Build}").ToList();

                        if (frameworkNames.Any() && frameworkNames.All(fn => fn == frameworkNames.First()))
                        {
                            var framework = frameworks.FirstOrDefault();
                            if (framework.FrameworkName.Framework.Equals(CoreDotNetFramework))
                            {
                                ResolvedFramework = $"{framework.FrameworkName.Framework.ToLower().TrimStart('.')}{framework.FrameworkName.Version.Major}.{framework.FrameworkName.Version.Minor}";
                            }
                            else if (framework.FrameworkName.Framework.Equals(FullDotNetFramework))
                            {
                                ResolvedFramework = $"v{framework.FrameworkName.Version.Major}.{framework.FrameworkName.Version.Minor}";
                                if (framework.FrameworkName.Version.Build != 0)
                                {
                                    ResolvedFramework = $"{ResolvedFramework}.{framework.FrameworkName.Version.Build}";
                                }

                            }
                        }
                    }
                }
            }
        }

        private Task OnExecute(CommandLineApplication app)
        {
            Console.WriteLine("\n");
            Console.WriteLine("Resolved solution: " + ResolvedSoluiton);
            Console.WriteLine("Resolved Name:" + ResolvedName);
            Console.WriteLine("Resolved Framework:" + ResolvedFramework);

            return Task.CompletedTask;
        }

        private string GetCommonValue(IList<string> values)
        {
            var byCharacters = new char[] { '.', '-', '_' };

            if (values == null || !values.Any())
            {
                return string.Empty;
            }

            if (values.Count == 1)
            {
                var value = values.First();
                var valueParts = value.Split(byCharacters);
                return valueParts.FirstOrDefault();
            }

            var diffSection = Diff.CalculateSections(values[0].ToCharArray(), values[1].ToCharArray()).ToList();
            if (!diffSection.IsNullOrEmpty())
            {
                var firstdiffSection = diffSection.FirstOrDefault();
                if (firstdiffSection.IsMatch)
                {
                    var prefix = values[0].Substring(0, firstdiffSection.LengthInCollection1).Trim('.');
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        var valueParts = prefix.Split(byCharacters);
                        return valueParts.FirstOrDefault();
                    }
                }
            }
            return string.Empty;
        }
    }
}