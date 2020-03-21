using System;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NSeed.Xunit
{
    /// <summary>
    /// TODO.
    /// </summary>
    /// <typeparam name="TScenario">TODO.TODO.</typeparam>
    /// <typeparam name="TSeedingStartup">TODO--.</typeparam>
    public class RequiresScenarios<TScenario, TSeedingStartup> : RequiresScenariosFixture
        where TScenario : IScenario
        where TSeedingStartup : SeedBucketStartup
    {
        public RequiresScenarios()
            : base(typeof(TSeedingStartup), typeof(TScenario))
        {
        }
    }

    /// <summary>
    /// TODO.
    /// </summary>
    /// <typeparam name="TScenario1">TODO.TODO.</typeparam>
    /// <typeparam name="TScenario2">TODO.TODO.TODO.</typeparam>
    /// <typeparam name="TSeedingStartup">TODO--.</typeparam>
#pragma warning disable SA1402 // File may only contain a single type
    public class RequiresScenarios<TScenario1, TScenario2, TSeedingStartup> : RequiresScenariosFixture
#pragma warning restore SA1402 // File may only contain a single type
        where TScenario1 : IScenario
        where TScenario2 : IScenario
        where TSeedingStartup : SeedBucketStartup
    {
        public RequiresScenarios()
            : base(typeof(TSeedingStartup), typeof(TScenario1), typeof(TScenario2))
        {
        }
    }

    /// <summary>
    /// TODO.
    /// </summary>
    /// <typeparam name="TScenario1">TODO.TODO.</typeparam>
    /// <typeparam name="TScenario2">TODO.TODO.TODO.</typeparam>
    /// <typeparam name="TScenario3">TODO----.</typeparam>
    /// <typeparam name="TSeedingStartup">TODO--.</typeparam>
#pragma warning disable SA1402 // File may only contain a single type
    public class RequiresScenarios<TScenario1, TScenario2, TScenario3, TSeedingStartup> : RequiresScenariosFixture
#pragma warning restore SA1402 // File may only contain a single type
        where TScenario1 : IScenario
        where TScenario2 : IScenario
        where TScenario3 : IScenario
        where TSeedingStartup : SeedBucketStartup
    {
        public RequiresScenarios()
            : base(typeof(TSeedingStartup), typeof(TScenario1), typeof(TScenario2), typeof(TScenario3))
        {
        }
    }
}
