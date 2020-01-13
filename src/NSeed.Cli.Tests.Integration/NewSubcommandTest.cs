using NSeed.Cli.Tests.Integration.Fixtures;
using System;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;
using static NSeed.Cli.Assets.Resources.New;
using static NSeed.Cli.Tests.Integration.Thens;

namespace NSeed.Cli.Tests.Integration
{
    [Collection("NSeedCollection")]
    public class NewSubcommandToolTest : IDisposable
    {
        private SearchSolutionPathErrors SearchSolutionPathErrors { get; } = SearchSolutionPathErrors.Instance;

        private NSeedFixture NSeed { get; }

        public NewSubcommandToolTest(NSeedFixture nSeedFixture)
        {
            NSeed = nSeedFixture;
            NSeed.CopyIntegrationTestScenariosToTempFolder();
        }

        [Fact]
        public void NSeedDll_Empty()
        {
            NSeed.Run(string.Empty);

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowHelpMessageForNSeedCommand();
        }

        [Fact]
        public void NSeedDll_HelpOption()
        {
            NSeed.Run("--help");

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowHelpMessageForNSeedCommand();
        }

        [Fact]
        public void NSeedDll_New_HelpOption()
        {
            NSeed.Run("new --help");

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowHelpMessageForNewSubcommand();
        }

        [Fact]
        public void NseedDll_New_Empty()
        {
            NSeed.Run("new");

            Then(NSeed.Response)
                .ShouldNotBeSuccessful(SearchSolutionPathErrors.WorkingDirectoryDoesNotContainAnyFile);
        }

        [Fact]
        public void NseedDll_New_NoSolution()
        {
            NSeed.Run("new --solution", NSeed.Scenario("EmptyFolder"));

            Then(NSeed.Response)
               .ShouldNotBeSuccessful(SearchSolutionPathErrors.WorkingDirectoryDoesNotContainAnyFile);
        }

        [Fact]
        public void NseedDll_New_SingleSolution_NoProjects()
        {
            NSeed.Run("new --solution", NSeed.Scenario("SingleSolution_NoProjects"));

            Then(NSeed.Response)
                .ShouldNotBeSuccessful(Errors.FrameworkNotProvided);
        }

        [Fact]
        public void NseedDll_New_SingleSolution_NoProjects_EmptyFramework()
        {
            NSeed.Run("new --solution", NSeed.Scenario("SingleSolution_NoProjects"), "--framework");

            Then(NSeed.Response)
              .ShouldNotBeSuccessful(argOutput: "Missing value for option 'framework'.");
        }

        [Fact]
        public void NseedDll_New_SingleSolution_NoProjects_ProvidedFramework()
        {
            NSeed.Run("new --solution", NSeed.Scenario("SingleSolution_NoProjects"), "-f netcoreapp2.2");

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowSuccessMessage("Seeds")
                .ShouldContainSeedBucketProject("Seeds", Path.Combine(NSeed.IntegrationTestScenariosTempFolderPath, "SingleSolution_NoProjects", "MyEmptySolution.sln"));
        }

        [Fact]
        public void NseedDll_New_SingleSolution_ExistingSeedProject_ProvidedFramework()
        {
            NSeed.Run("new --solution ", NSeed.Scenario("SingleSolution_WithExistingSeedsProject"),
                "-f netcoreapp2.2");

            Then(NSeed.Response)
                .ShouldNotBeSuccessful(Errors.ProjectNameExists);
        }

        [Fact]
        public void NseedDll_New_MultipleSolutions()
        {
            NSeed.Run("new --solution", NSeed.Scenario("MultipleSolutions"), "-f netcoreapp2.2");

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowSuccessMessage("Seeds")
                .ShouldContainSeedBucketProject("Seeds", Path.Combine(NSeed.IntegrationTestScenariosTempFolderPath, "MultipleSolutions", "MainSolution.sln"));
        }

        [Fact]
        public void NseedDll_New_MultipleSolutions_InSubFolder()
        {
            NSeed.Run("new --solution ", NSeed.Scenario("MultipleSolutions_InSubFolder"), "-f netcoreapp2.2");

            Then(NSeed.Response)
               .ShouldNotBeSuccessful(SearchSolutionPathErrors.MultipleFilesFound);
        }

        [SkippableFact]
        public void NseedDll_New_SingleSolutions_WithMultipleProjects_DifferentFrameworks()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

            NSeed.Run("new --solution", NSeed.Scenario("SingleSolution_WithMultipleProjectWithDifferentFrameworks"),
                "-n MyCustomProjectName");

            Then(NSeed.Response)
               .ShouldNotBeSuccessful(Errors.FrameworkNotProvided);
        }

        [SkippableFact]
        public void NseedDll_New_SingleSolution_ExistingDotNetClassicFrameworkProject()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

            NSeed.Run("new --solution ", NSeed.Scenario("SingleSolution_WithDotNetClassicFramework"));

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
