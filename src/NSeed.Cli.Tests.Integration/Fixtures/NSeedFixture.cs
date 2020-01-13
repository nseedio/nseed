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

            var repositoryNseedFolderPath = Path.GetFullPath(Path.Combine(RepositorySrcFolderPath, ".."));

            NSeedTestsIntegrationFolderPath = Path.Combine(repositoryNseedFolderPath, "tests", "integration");

            ToolNupkgPath = Path.Combine(RepositorySrcFolderPath, "NSeed.Cli", "bin", "Debug");

            IntegrationTestScenariosTempFolderPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}_NSeedIntegrationTestScenarios");

            Runner.RunDotNet(RepositorySrcFolderPath, new string[]
            {
                  @$"tool install -g --version 0.2.0 --add-source {ToolNupkgPath} NSeed.Cli"
            });
        }

        public void Dispose()
        {
            Runner.RunDotNet(RepositorySrcFolderPath, new string[]
            {
                "tool uninstall --global NSeed.Cli"
            });

            ClearTestData();
        }

        public void LogToLinuxConsole(string message)
        {
            using var writer = new StreamWriter(Console.OpenStandardOutput());
            writer.WriteLine($"Test Log: {message}");
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
            Response = Runner.RunNSeed(command);
            LogToLinuxConsole($"Output: {Response.Output} Error: {Response.Errors} IsSuccess: {Response.IsSuccess} ExitCode:{Response.ExitCode}");
        }

        internal string Scenario(string scenario)
        {
            return Path.Combine(IntegrationTestScenariosTempFolderPath, scenario);
        }

        internal RunStatus Response { get; private set; } = new RunStatus();

        internal string IntegrationTestScenariosTempFolderPath { get; }

        private string ToolNupkgPath { get; }

        private string NSeedTestsIntegrationFolderPath { get; }

        private TestRunner Runner { get; }

        private string RepositorySrcFolderPath { get; }

        private string CurrentAssemblyPath { get; } = Assembly.GetExecutingAssembly().Location;
    }
}
