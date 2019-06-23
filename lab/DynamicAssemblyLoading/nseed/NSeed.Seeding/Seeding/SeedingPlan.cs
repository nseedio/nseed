using System;
using System.Collections.Generic;
using NSeed.Seeding.Extensions;
using static System.Diagnostics.Debug;

namespace NSeed.Seeding
{
    public class SeedingPlan
    {
        public ISeedingSetup SeedingSetup { get; }
        public Type SeedingSetupType { get; }
        public List<SeedInfo> Seeds { get; }

        internal SeedingPlan(ISeedingSetup seedingSetup, Type seedingSetupType, List<SeedInfo> seeds)
        {
            if (seedingSetupType != null) Assert(seedingSetupType.IsSeedingSetupType());
            Assert(seeds != null);

            SeedingSetup = seedingSetup;
            SeedingSetupType = seedingSetupType;
            Seeds = seeds;
        }
    }
}