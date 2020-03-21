using NSeed;
using NSeed.Xunit;
using System;
using System.Collections.Generic;
using System.Text;

namespace GettingThingsDone.ApplicationCore.Tests.Unit
{
    internal class UseSeeds<TSeed> : RequiresSeeds<TSeed, SampleStartupForUnitTests>
        where TSeed : ISeed
    {
    }

#pragma warning disable SA1402 // File may only contain a single type
    internal class UseSeeds<TSeed1, TSeed2> : RequiresSeeds<TSeed1, TSeed2, SampleStartupForUnitTests>
#pragma warning restore SA1402 // File may only contain a single type
        where TSeed1 : ISeed
        where TSeed2 : ISeed
    {
    }
}
