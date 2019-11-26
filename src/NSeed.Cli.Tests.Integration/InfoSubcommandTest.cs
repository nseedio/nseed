using NSeed.Cli.Assets;
using NSeed.Cli.Tests.Integration.Fixtures;
using System;
using System.IO;
using Xunit;
using static NSeed.Cli.Assets.Resources.Info;
using static NSeed.Cli.Tests.Integration.Thens;

namespace NSeed.Cli.Tests.Integration
{
    public class InfoSubcommandTest : IClassFixture<NSeedFixture>, IDisposable
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
            NSeed.Run("NSeed.Cli.dll info --help");

            Then(NSeed.Response)
                .ShouldBeSuccessful()
                .ShouldShowHelpMessageForInfoSubcommand();
        }

        [Fact]
        public void NSeedDll_Info()
        {
            NSeed.Run("NSeed.Cli.dll info");

            Then(NSeed.Response)
                .ShouldNotBeSuccessful(Errors.WorkingDirectoryDoesNotContainAnyFile);
        }

        [Fact]
        public void NseedDll_Info_ClassicConsoleApp_NoSeedProject()
        {
            NSeed.Run("NSeed.Cli.dll info --project ", NSeed.Scenario("NetClassicConsoleApp"));

            Then(NSeed.Response)
                .ShouldNotBeSuccessful(Resources.Info.Errors.NSeedProjectCouldNotBeFound);
        }

        public void Dispose()
        {
            NSeed.ClearTestData();
        }
    }
}
