using FluentAssertions;
using NSeed.Cli.Tests.Integration.Fixtures;
using System;
using System.IO;
using Xunit;
using static NSeed.Cli.Assets.Resources.New;
using static NSeed.Cli.Tests.Integration.Thens;

namespace NSeed.Cli.Tests.Integration
{
    public class NewSubcommandToolTest : IClassFixture<NSeedFixture>, IDisposable
    {
        private SearchSolutionPathErrors SearchSolutionPathErrors { get; } = SearchSolutionPathErrors.Instance;

        private NSeedFixture NSeed { get; }

        public NewSubcommandToolTest(NSeedFixture nSeedFixture)
        {
            NSeed = nSeedFixture;
            NSeed.CopyIntegrationTestScenariosToTempFolder();
        }

        // [Fact]
        // public void NSeedDllInstalledTool_Empty()
        // {
        //     var response = NSeedFixture.Runner.RunNSeed(NSeedFixture.SrcFolderPath, new string[] { });
        //     OutputShouldBeSuccessful(response);
        //     // Todo am Comment with Igor -> This is very strange thing for me install tool is always older version of tool because I couldn't clear Nuget cache.
        //     // OutputShouldShowHelpMessage(response);
        // }

        [Fact]
        public void NSeedDll_Empty()
        {
            NSeed.Run("NSeed.Cli.dll");

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowHelpMessageForNSeedCommand();
        }

        [Fact]
        public void NSeedDll_HelpOption()
        {
            NSeed.Run("NSeed.Cli.dll --help");

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowHelpMessageForNSeedCommand();
        }

        [Fact]
        public void NSeedDll_New_HelpOption()
        {
            NSeed.Run("NSeed.Cli.dll new --help");

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowHelpMessageForNewSubcommand();
        }

        [Fact]
        public void NseedDll_New_Empty()
        {
            NSeed.Run("NSeed.Cli.dll new");

            Then(NSeed.Response)
                .ShouldNotBeSuccessful(SearchSolutionPathErrors.WorkingDirectoryDoesNotContainAnyFile);
        }

        [Fact]
        public void NseedDll_New_NoSolution()
        {
            NSeed.Run(
                "NSeed.Cli.dll new --solution ",
                NSeed.Scenario("EmptyFolder"));

            Then(NSeed.Response)
               .ShouldNotBeSuccessful(SearchSolutionPathErrors.WorkingDirectoryDoesNotContainAnyFile);
        }

        [Fact]
        public void NseedDll_New_SingleSolution_NoProjects()
        {
            NSeed.Run(
                "NSeed.Cli.dll new --solution ",
                NSeed.Scenario("SingleSolution_NoProjects"));

            Then(NSeed.Response)
                .ShouldNotBeSuccessful(Errors.FrameworkNotProvided);
        }

        [Fact]
        public void NseedDll_New_SingleSolution_NoProjects_EmptyFramework()
        {
            NSeed.Run(
                "NSeed.Cli.dll new --solution ",
                NSeed.Scenario("SingleSolution_NoProjects"),
                "--framework");

            Then(NSeed.Response)
              .ShouldNotBeSuccessful(output: "Specify --help for a list of available options and commands.\r\nMissing value for option 'framework'.\r\n");
        }

        [Fact]
        public void NseedDll_New_SingleSolution_NoProjects_ProvidedFramework()
        {
            NSeed.Run(
                "NSeed.Cli.dll new --solution ",
                NSeed.Scenario("SingleSolution_NoProjects"),
                "-f netcoreapp2.2");

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowSuccessMessage("Seeds")
                .ShouldContainSeedBucketProject("Seeds", Path.Combine(NSeed.IntegrationTestScenariosTempFolderPath, "SingleSolution_NoProjects", "MyEmptySolution.sln"));
        }

        [Fact]
        public void NseedDll_New_SingleSolution_ExistingSeedProject_ProvidedFramework()
        {
            NSeed.Run(
                "NSeed.Cli.dll new --solution ",
                NSeed.Scenario("SingleSolution_WithExistingSeedsProject"),
                "-f netcoreapp2.2");

            Then(NSeed.Response)
                .ShouldNotBeSuccessful(Errors.ProjectNameExists);
        }

        [Fact]
        public void NseedDll_New_MultipleSolutions()
        {
            NSeed.Run(
                "NSeed.Cli.dll new --solution ",
                NSeed.Scenario("MultipleSolutions"),
                "-f netcoreapp2.2");

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowSuccessMessage("Seeds")
                .ShouldContainSeedBucketProject("Seeds", Path.Combine(NSeed.IntegrationTestScenariosTempFolderPath, "MultipleSolutions", "MainSolution.sln"));
        }

        [Fact]
        public void NseedDll_New_MultipleSolutions_InSubFolder()
        {
            NSeed.Run(
                "NSeed.Cli.dll new --solution ",
                NSeed.Scenario("MultipleSolutions_InSubFolder"),
                "-f netcoreapp2.2");

            Then(NSeed.Response)
               .ShouldNotBeSuccessful(SearchSolutionPathErrors.MultipleFilesFound);
        }

        [Fact]
        public void NseedDll_New_SingleSolutions_WithMultipleProjects_DifferentFrameworks()
        {
            NSeed.Run(
                "NSeed.Cli.dll new --solution ",
                NSeed.Scenario("SingleSolution_WithMultipleProjectWithDifferentFrameworks"),
                "-n MyCustomProjectName");

            Then(NSeed.Response)
               .ShouldNotBeSuccessful(Errors.FrameworkNotProvided);
        }

        [Fact]
        public void NseedDll_New_SingleSolution_ExistingDotNetClassicFrameworkProject()
        {
            NSeed.Run(
               "NSeed.Cli.dll new --solution ",
               NSeed.Scenario("SingleSolution_WithDotNetClassicFramework"));

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowSuccessMessage("ConsoleApp.Seeds", shouldShowMissingNugetPackageWarning: true)
                .ShouldContainSeedBucketProject("ConsoleApp.Seeds", Path.Combine(NSeed.IntegrationTestScenariosTempFolderPath, "SingleSolution_WithDotNetClassicFramework", "MyEmptySolution.sln"));
        }

        public void Dispose()
        {
            NSeed.ClearTestData();
        }
    }
}
