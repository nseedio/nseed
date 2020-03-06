using NSeed.MetaInfo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.Seeding
{
    internal class SeedingReport
    {
        public SeedingStatus Status { get; }

        public SeedBucketInfo SeedBucketInfo { get; }

        public SeedingPlan? SeedingPlan { get; }

        public IReadOnlyCollection<SingleSeedSeedingReport> SingleSeedSeedingResults { get; }

        private SeedingReport(SeedBucketInfo seedBucketInfo, SeedingPlan? seedingPlan, IReadOnlyCollection<SingleSeedSeedingReport> singleSeedSeedingResults, SeedingStatus status)
        {
            System.Diagnostics.Debug.Assert(status != SeedingStatus.SeedBucketHasErrors ||
                (status == SeedingStatus.SeedBucketHasErrors &&
                 seedBucketInfo.HasAnyErrors &&
                 seedingPlan == null &&
                 singleSeedSeedingResults.Count == 0));

            System.Diagnostics.Debug.Assert(status != SeedingStatus.BuildingServiceProviderFailed ||
                (status == SeedingStatus.BuildingServiceProviderFailed &&
                 !seedBucketInfo.HasAnyErrors &&
                 seedingPlan != null &&
                 singleSeedSeedingResults.Count == 0));

            System.Diagnostics.Debug.Assert(status != SeedingStatus.SeedingSingleSeedFailed ||
                (status == SeedingStatus.SeedingSingleSeedFailed &&
                 !seedBucketInfo.HasAnyErrors &&
                 seedingPlan != null &&
                 singleSeedSeedingResults.Count(result => result.Status == SingleSeedSeedingStatus.Failed) == 1 &&
                 singleSeedSeedingResults.Last().Status == SingleSeedSeedingStatus.Failed));

            System.Diagnostics.Debug.Assert(status != SeedingStatus.Succeeded ||
                (status == SeedingStatus.Succeeded &&
                 !seedBucketInfo.HasAnyErrors &&
                 seedingPlan != null &&

                 // We can have a situation that there is no seeds to seed. This is fine.
                 singleSeedSeedingResults.All(result => result.Status != SingleSeedSeedingStatus.Failed)));

            SeedBucketInfo = seedBucketInfo;
            SeedingPlan = seedingPlan;
            SingleSeedSeedingResults = singleSeedSeedingResults;
            Status = status;
        }

        public static SeedingReport CreateForSeedBucketHasErrors(SeedBucketInfo seedBucketInfo)
        {
            return new SeedingReport
            (
                seedBucketInfo,
                null,
                Array.Empty<SingleSeedSeedingReport>(),
                SeedingStatus.SeedBucketHasErrors
            );
        }

        public static SeedingReport CreateForBuildingServiceProviderFailed(SeedBucketInfo seedBucketInfo, SeedingPlan seedingPlan)
        {
            return new SeedingReport
            (
                seedBucketInfo,
                seedingPlan,
                Array.Empty<SingleSeedSeedingReport>(),
                SeedingStatus.BuildingServiceProviderFailed
            );
        }

        public static SeedingReport CreateForSeedingSingleSeedFailed(SeedBucketInfo seedBucketInfo, SeedingPlan seedingPlan, IReadOnlyCollection<SingleSeedSeedingReport> singleSeedSeedingResults)
        {
            return new SeedingReport
            (
                seedBucketInfo,
                seedingPlan,
                singleSeedSeedingResults,
                SeedingStatus.SeedingSingleSeedFailed
            );
        }

        public static SeedingReport CreateForSucceeded(SeedBucketInfo seedBucketInfo, SeedingPlan seedingPlan, IReadOnlyCollection<SingleSeedSeedingReport> singleSeedSeedingResults)
        {
            return new SeedingReport
            (
                seedBucketInfo,
                seedingPlan,
                singleSeedSeedingResults,
                SeedingStatus.Succeeded
            );
        }
    }
}
