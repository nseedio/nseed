using FluentAssertions;
using NSeed.Cli.Assets;
using NSeed.Cli.Runners;
using NSeed.Cli.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Xunit;

namespace NSeed.Cli.Tests.Integration
{
    public class NSeedFixture : IDisposable
    {
        internal string SrcFolderPath { get; }

        internal string ToolDllPath { get; }

        internal string IntegrationTestScenariosTempFolderPath { get; }

        internal TestRunner Runner { get; }

        private string IntegrationTestScenariosPath { get; }

        private string CurrentAssemblyPath { get; }

        private string ToolNupkgPath { get; }

        private string NugetNSeedPackageCachePath { get; }

        public NSeedFixture()
        {
            Runner = new TestRunner();
            CurrentAssemblyPath = Assembly.GetExecutingAssembly().Location;
            SrcFolderPath = Path.GetFullPath(Path.Combine(CurrentAssemblyPath, "..", "..", "..", "..", ".."));
            IntegrationTestScenariosPath = Path.Combine(CurrentAssemblyPath, "..", "..", "..", "..", "..", "..");
            ToolNupkgPath = Path.Combine(SrcFolderPath, "NSeed.Cli", "bin", "Debug");
            ToolDllPath = Path.Combine(ToolNupkgPath, "netcoreapp2.2");
            NugetNSeedPackageCachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages", "nseed");
            IntegrationTestScenariosTempFolderPath = Path.Combine(Path.GetTempPath(), "004332117_NSeedIntegrationTestScenarios");

            CopyIntegrationTestScenariosToTempFolder();

            if (Directory.Exists(NugetNSeedPackageCachePath))
            {
                Directory.Delete(NugetNSeedPackageCachePath, true);
            }

            var installToolResponse = Runner.Run(SrcFolderPath, new string[]
            {
                 @$"tool install -g --add-source {ToolNupkgPath} NSeed.Cli"
            });
        }

        public void CopyIntegrationTestScenariosToTempFolder()
        {
            if (Directory.Exists(IntegrationTestScenariosTempFolderPath))
            {
                Directory.Delete(IntegrationTestScenariosTempFolderPath, true);
            }
            string fullScenarioDirectoryPath = Path.Combine(IntegrationTestScenariosPath, "tests", "integration");
            FileSystemService.DirectoryCopy(fullScenarioDirectoryPath, IntegrationTestScenariosTempFolderPath, true);
        }

        public void Dispose()
        {
            Runner.Run(SrcFolderPath, new string[]
            {
                "tool uninstall --global NSeed.Cli"
            });

            if (Directory.Exists(IntegrationTestScenariosTempFolderPath))
            {
                Directory.Delete(IntegrationTestScenariosTempFolderPath, true);
            }
        }
    }

    public class NewSubcommandToolTest : IClassFixture<NSeedFixture>
    {
        private readonly NSeedFixture nSeedFixture;

        public NewSubcommandToolTest(NSeedFixture nSeedFixture)
        {
            this.nSeedFixture = nSeedFixture;
        }

        [Fact]
        public void NSeedDllInstalledTool_Empty()
        {
            var response = nSeedFixture.Runner.RunNSeed(nSeedFixture.SrcFolderPath, new string[] { });
            OutputShouldBeSuccessful(response);
            OutputShouldShowHelpMessage(response);
        }

        [Fact]
        public void NSeedDll_Empty()
        {
            var response = nSeedFixture.Runner.Run(nSeedFixture.ToolDllPath, new string[]
            {
                "NSeed.Cli.dll"
            });

            OutputShouldBeSuccessful(response);
            OutputShouldShowHelpMessage(response);
        }

        [Fact]
        public void NSeedDll_HelpOption()
        {
            var response = nSeedFixture.Runner.Run(nSeedFixture.ToolDllPath, new string[]
            {
                "NSeed.Cli.dll --help"
            });

            OutputShouldBeSuccessful(response);
            OutputShouldShowHelpMessage(response);
        }

        [Fact]
        public void NseedDll_NewSubcommand_Empty()
        {
            var response = nSeedFixture.Runner.Run(nSeedFixture.ToolDllPath, new string[]
            {
                "NSeed.Cli.dll new"
            });
            OutputShouldNotBeSuccessful(response);
            OutputShouldShowHintMessage(response);
            OutputShouldContainError(response, Resources.New.Errors.WorkingDirectoryDoesNotContainAnySolution);
        }

        [Fact]
        public void NseedDll_NewSubcommand_NoSolution()
        {
            var path = Path.Combine(nSeedFixture.IntegrationTestScenariosTempFolderPath, "EmptyFolder");

            var response = nSeedFixture.Runner.Run(nSeedFixture.ToolDllPath, new string[]
            {
                "NSeed.Cli.dll new --solution ", path
            });

            OutputShouldNotBeSuccessful(response);
            OutputShouldShowHintMessage(response);
            OutputShouldContainError(response, Resources.New.Errors.WorkingDirectoryDoesNotContainAnySolution);
        }

        [Fact]
        public void NseedDll_NewSubcommand_SingleSolution_NoProjects()
        {
            nSeedFixture.CopyIntegrationTestScenariosToTempFolder();

            var path = Path.Combine(nSeedFixture.IntegrationTestScenariosTempFolderPath, "SingleSolution_NoProjects");

            var response = nSeedFixture.Runner.Run(nSeedFixture.ToolDllPath, new string[]
            {
                "NSeed.Cli.dll new --solution ", path
            });
            OutputShouldNotBeSuccessful(response);
            OutputShouldShowHintMessage(response);
            OutputShouldContainError(response, Resources.New.Errors.FrameworkNotProvided);
        }

        [Fact]
        public void NseedDll_NewSubcommand_SingleSolution_NoProjects_EmptyFramework()
        {
            nSeedFixture.CopyIntegrationTestScenariosToTempFolder();

            var path = Path.Combine(nSeedFixture.IntegrationTestScenariosTempFolderPath, "SingleSolution_NoProjects");

            var response = nSeedFixture.Runner.Run(nSeedFixture.ToolDllPath, new string[]
            {
                "NSeed.Cli.dll new --solution ", path, "--framework"
            });
            OutputShouldNotBeSuccessful(response);
            OutputShouldShowHintMessageForMissingFramework(response);
        }

        [Fact]
        public void NseedDll_NewSubcommand_SingleSolution_NoProjects_ProvidedFramework()
        {
            nSeedFixture.CopyIntegrationTestScenariosToTempFolder();

            var path = Path.Combine(nSeedFixture.IntegrationTestScenariosTempFolderPath, "SingleSolution_NoProjects");

            var response = nSeedFixture.Runner.Run(nSeedFixture.ToolDllPath, new string[]
            {
                "NSeed.Cli.dll new --solution ", path, "-f netcoreapp2.2"
            });
            OutputShouldBeSuccessful(response);
            OutputShouldShowSuccessMessage(response);
        }

        [Fact]
        public void NseedDll_NewSubcommand_SingleSolution_ExistingSeedProject_ProvidedFramework()
        {
            var path = Path.Combine(nSeedFixture.IntegrationTestScenariosTempFolderPath, "SingleSolution_WithExistingSeedsProject");

            var response = nSeedFixture.Runner.Run(nSeedFixture.ToolDllPath, new string[]
            {
                "NSeed.Cli.dll new --solution ", path, "-f netcoreapp2.2"
            });
        }

        private void OutputShouldBeSuccessful(RunStatus status)
        {
            status.IsSuccess.Should().BeTrue();
        }

        private void OutputShouldNotBeSuccessful(RunStatus status)
        {
            status.IsSuccess.Should().BeFalse();
        }

        private void OutputShouldShowHelpMessage(RunStatus status)
        {
            status.Output.Should().StartWith("Data seeding command line tool.");
            status.Output.Should().Contain("Usage:");
            status.Output.Should().EndWith("Run 'NSeed [command] --help' for more information about a command.\r\n\r\n");
        }

        private void OutputShouldShowHintMessage(RunStatus status)
        {
            status.Output.Should().Be("Specify --help for a list of available options and commands.\r\n");
        }

        private void OutputShouldShowHintMessageForMissingFramework(RunStatus status)
        {
            status.Output.Should().Be("Specify --help for a list of available options and commands.\r\nMissing value for option 'framework'.\r\n");
        }

        private void OutputShouldContainError(RunStatus status, string errorMessage)
        {
            status.Errors.Should().BeEquivalentTo($"{errorMessage}\r\n");
        }

        private void OutputShouldShowSuccessMessage(RunStatus status)
        {
            status.Output.Should().BeEquivalentTo($"{Resources.New.SuccessfulRun}\r\n");
        }
    }

    internal class TestRunner : DotNetRunner
    {
        public new RunStatus Run(string workingDirectory, string[] arguments)
        {
            return base.Run(workingDirectory, arguments);
        }

        public RunStatus RunNSeed(string workingDirectory, string[] arguments)
        {
            var nseedToolExePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".dotnet", "tools", "nseed.exe");

            var psi = new ProcessStartInfo(nseedToolExePath, string.Join(" ", arguments))
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            return Run(psi);
        }
    }
}
