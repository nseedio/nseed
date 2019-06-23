namespace NSeed.Contracts
{
    public class Ensure
    {
        private Ensure() { }

        public static readonly Ensure Default = new Ensure();

        public void That(bool condition, string errorMessage = null)
        {
            if (!condition) throw new SeedContractViolationException(errorMessage);
        }

        public void NotifyError(string errorMessage = null)
        {
            throw new SeedContractViolationException(errorMessage);
        }
    }
}