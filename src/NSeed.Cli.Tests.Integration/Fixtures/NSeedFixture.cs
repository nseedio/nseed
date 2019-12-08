using NSeed.Cli.Runners;
using NSeed.Cli.Services;
using NSeed.Cli.Tests.Integration.Runners;
using System;
using System.IO;
using System.Reflection;

namespace NSeed.Cli.Tests.Integration.Fixtures
{
    public class NSeedFixture : IDisposable
    {
        public NSeedFixture()
        {
            Runner = new TestRunner();

            RepositorySrcFolderPath = Path.GetFullPath(Path.Combine(CurrentAssemblyPath, "..", "..", "..", "..", ".."));
            NSeedTestsIntegrationFolderPath = GetNSeedTestsIntegrationFolderPath();

            ToolNupkgPath = Path.Combine(RepositorySrcFolderPath, "NSeed.Cli", "bin", "Debug");
            // TODO This will not work if we change framework version.
            ToolDllPath = Path.Combine(ToolNupkgPath, "netcoreapp2.2");
            // NugetNSeedPackageCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages", "nseed");

            IntegrationTestScenariosTempFolderPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}_NSeedIntegrationTestScenarios");

            // var installToolResponse = Runner.RunDotNet(RepositorySrcFolderPath, new string[]
            // {
            //      @$"tool install -g --add-source {ToolNupkgPath} NSeed.Cli"
            // });
        }

        public void Dispose()
        {
            Runner.RunDotNet(RepositorySrcFolderPath, new string[]
            {
                "tool uninstall --global NSeed.Cli"
            });

            ClearTestData();
        }

        internal void CopyIntegrationTestScenariosToTempFolder()
        {
            ClearTestData();
            FileSystemService.DirectoryCopy(NSeedTestsIntegrationFolderPath, IntegrationTestScenariosTempFolderPath, true);
        }

        internal void ClearTestData()
        {
            if (Directory.Exists(IntegrationTestScenariosTempFolderPath))
            {
                Directory.Delete(IntegrationTestScenariosTempFolderPath, true);
            }
        }

        internal void Run(params string[] command)
        {
            Response = Runner.RunDotNet(ToolDllPath, command);
        }

        internal void Run(string command)
        {
            Response = Runner.RunDotNet(ToolDllPath, new string[] { command });
        }

        internal string Scenario(string scenario)
        {
            return Path.Combine(IntegrationTestScenariosTempFolderPath, scenario);
        }

        internal RunStatus Response { get; private set; } = new RunStatus();

        internal string IntegrationTestScenariosTempFolderPath { get; }

        private string ToolNupkgPath { get; }

        private string NSeedTestsIntegrationFolderPath { get; }

        // private string NugetNSeedPackageCachePath { get; }

        private string ToolDllPath { get; }

        private TestRunner Runner { get; }

        private string RepositorySrcFolderPath { get; }

        private string CurrentAssemblyPath { get; } = Assembly.GetExecutingAssembly().Location;

        private string GetNSeedTestsIntegrationFolderPath()
        {
            var temp = Path.Combine(CurrentAssemblyPath, "..", "..", "..", "..", "..", "..");
            return Path.Combine(temp, "tests", "integration");
        }
    }
}
