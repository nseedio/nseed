using NSeed.Filtering;
using NSeed.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Seeding
{
    internal class SeedingPlan
    {
        private static readonly SeedingPlan Empty = new SeedingPlan(Array.Empty<SeedInfo>());

        public IReadOnlyList<SeedInfo> SeedingSteps { get; }

        private SeedingPlan(IReadOnlyList<SeedInfo> seedingSteps)
        {
            SeedingSteps = seedingSteps;
        }

        public static SeedingPlan CreateFor(SeedBucketInfo seedBucketInfo, ISeedableFilter filter)
        {
            System.Diagnostics.Debug.Assert(!seedBucketInfo.HasAnyErrors);

            if (!seedBucketInfo.ContainedSeedables.Any(seedable => filter.Accepts(seedable)))
                return Empty;

            var alwaysRequiredSeeds = seedBucketInfo.ContainedSeedables.OfType<SeedInfo>().Where(seed => seed.IsAlwaysRequired);
            var filteredSeedables = seedBucketInfo.ContainedSeedables.Where(seedable => filter.Accepts(seedable));
            var seedablesToSeed = filteredSeedables.Union(alwaysRequiredSeeds);

            var seedingSteps = new List<SeedInfo>(seedBucketInfo.ContainedSeedables.OfType<SeedInfo>().Count());
            foreach (var seedableInfo in seedablesToSeed)
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
