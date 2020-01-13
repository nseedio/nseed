using NSeed.Cli.Assets;
using NSeed.Cli.Tests.Integration.Fixtures;
using System;
using System.Runtime.InteropServices;
using Xunit;
using static NSeed.Cli.Assets.Resources.Info;
using static NSeed.Cli.Tests.Integration.Thens;

namespace NSeed.Cli.Tests.Integration
{
    [Collection("NSeedCollection")]
    public class InfoSubcommandTest : IDisposable
    {
        private NSeedFixture NSeed { get; }

        private SearchNSeedProjectPathErrors Errors { get; }

        public InfoSubcommandTest(NSeedFixture nSeedFixture)
        {
            NSeed = nSeedFixture;
            NSeed.CopyIntegrationTestScenariosToTempFolder();
            Errors = SearchNSeedProjectPathErrors.Instance;
        }

        [Fact]
        public void NSeedDll_Info_HelpOption()
        {
            NSeed.Run("info --help");

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowHelpMessageForInfoSubcommand();
        }

        [Fact]
        public void NSeedDll_Info()
        {
            NSeed.Run("info");

            Then(NSeed.Response)
                .ShouldNotBeSuccessful(Errors.WorkingDirectoryDoesNotContainAnyFile);
        }

        [Fact]
        public void NSeedDll_ProjectNotDefine()
        {
            NSeed.Run("info --project");

            Then(NSeed.Response)
                .ShouldNotBeSuccessful(argOutput: "Missing value for option 'project'.");
        }

        [SkippableFact]
        public void NseedDll_Info_ClassicConsoleApp_NoSeedProject()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

            NSeed.Run("info --project ", NSeed.Scenario("NetClassicConsoleApp"));

            Then(NSeed.Response)
                .ShouldNotBeSuccessful(Resources.Info.Errors.SeedBucketProjectCouldNotBeFound);
        }

        [SkippableFact]
        public void NseedDll_Info_Classic_EmptySeedBucketProject()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

            NSeed.Run("info --project ", NSeed.Scenario("EmptySeedBucket.DotNetClassic"));

            Then(NSeed.Response)
                .ShouldBeSuccessful();
        }

        // TODO Don't skip this fact on LINUX after creating NSeed package 0.2.0 on Nuget.org
        [SkippableFact]
        public void NseedDll_Info_Core_EmptySeedBucketProject()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

            NSeed.Run("info --project ", NSeed.Scenario("EmptySeedBucket.DotNetCore"));

            Then(NSeed.Response)
                .ShouldBeSuccessful();
        }

        public void Dispose()
        {
            NSeed.ClearTestData();
        }
    }
}
