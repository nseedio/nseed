using NSeed;
using NSeed.Xunit;
using System;
using System.Collections.Generic;
using System.Text;

namespace GettingThingsDone.ApplicationCore.Tests.Unit
{
    internal class UseScenarios<TScenario> : RequiresScenarios<TScenario, SampleStartupForUnitTests>
        where TScenario : IScenario
    {
    }

#pragma warning disable SA1402 // File may only contain a single type
    internal class UseScenarios<TScenario1, TScenario2> : RequiresScenarios<TScenario1, TScenario2, SampleStartupForUnitTests>
#pragma warning restore SA1402 // File may only contain a single type
        where TScenario1 : IScenario
        where TScenario2 : IScenario
    {
    }
}
