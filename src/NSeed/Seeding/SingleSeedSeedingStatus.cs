namespace NSeed.Seeding
{
    internal enum SingleSeedSeedingStatus
    {
        Seeded,
        Skipped, // TODO: Or SeedingSkipped
        Failed // TODO: Or SeedingFailed

        // TODO: NotCreated or FailedToCreate or SeedNotCreated or SeedFailedToCreate or CreationOfSeedFailed
        // TODO: ConstraintValidated or SeedConstraintValidated
    }
}
