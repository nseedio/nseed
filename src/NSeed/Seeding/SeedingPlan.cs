using NSeed.MetaInfo;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Seeding
{
    internal class SeedingPlan
    {
        public IReadOnlyList<SeedInfo> SeedingSteps { get; }

        private SeedingPlan(IReadOnlyList<SeedInfo> seedingSteps)
        {
            SeedingSteps = seedingSteps;
        }

        public static SeedingPlan CreateFor(SeedBucketInfo seedBucketInfo)
        {
            System.Diagnostics.Debug.Assert(!seedBucketInfo.HasAnyErrors);

            var seedingSteps = new List<SeedInfo>(seedBucketInfo.ContainedSeedables.OfType<SeedInfo>().Count());
            foreach (var seedableInfo in seedBucketInfo.ContainedSeedables)
                RecursivelyBuildSeedingSteps(seedableInfo);

            return new SeedingPlan(seedingSteps);

            void RecursivelyBuildSeedingSteps(SeedableInfo seedableInfo)
            {
                if (seedableInfo is SeedInfo seedInfo && seedingSteps.Contains(seedInfo)) return;

                foreach (var requiredSeedable in seedableInfo.RequiredSeedables)
                    RecursivelyBuildSeedingSteps(requiredSeedable);

                // We have to add the seed info to the seeding plan at the
                // tail of the recursion so that the required seedables get
                // executed first (end up on the front of the seeding plan).
                if (seedableInfo is SeedInfo)
                    seedingSteps.Add((SeedInfo)seedableInfo);
            }
        }
    }
}
