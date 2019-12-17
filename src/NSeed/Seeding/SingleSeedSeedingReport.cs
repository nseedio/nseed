using NSeed.MetaInfo;

namespace NSeed.Seeding
{
    internal class SingleSeedSeedingReport
    {
        public SingleSeedSeedingStatus Status { get; }

        public SeedInfo SeedInfo { get; }

        public SingleSeedSeedingReport(SingleSeedSeedingStatus status, SeedInfo seedInfo)
        {
            Status = status;
            SeedInfo = seedInfo;
        }
    }
}
