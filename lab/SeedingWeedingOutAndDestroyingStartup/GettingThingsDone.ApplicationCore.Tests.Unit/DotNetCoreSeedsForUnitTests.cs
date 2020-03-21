using DotNetCoreSeeds;
using NSeed.Xunit;
using System;
using System.Collections.Generic;
using System.Text;

namespace GettingThingsDone.ApplicationCore.Tests.Unit
{
    internal class DotNetCoreSeedsForUnitTests : RequiresSeedBucket<DotNetCoreSeedsSeedBucket, SampleStartupForUnitTests>
    {
    }
}
