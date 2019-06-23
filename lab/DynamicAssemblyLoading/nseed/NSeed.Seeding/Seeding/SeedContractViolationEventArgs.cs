namespace NSeed.Seeding
{
    public class SeedContractViolationEventArgs : SeedingEventArgs
    {
        public string ErrorMessage { get; }

        public SeedContractViolationEventArgs(int seedingStep, SeedInfo seed, string errorMessage)
            : base(seedingStep, seed)
        {
            ErrorMessage = errorMessage;
        }
    }
}