using NSeed.Cli.Tests.Integration.Fixtures;
using Xunit;

namespace NSeed.Cli.Tests.Integration
{
    [CollectionDefinition("NSeedCollection")]
    public class NSeedCollection : ICollectionFixture<NSeedFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
