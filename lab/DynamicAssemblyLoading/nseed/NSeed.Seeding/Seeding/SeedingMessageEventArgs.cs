using static System.Diagnostics.Debug;

namespace NSeed.Seeding
{
    public class SeedingMessageEventArgs : SeedingEventArgs
    {
        public string Message { get; }

        internal SeedingMessageEventArgs(string message) : this(message, 0, null) { }

        internal SeedingMessageEventArgs(string message, int seedingStep, SeedInfo seed) : base(seedingStep, seed)
        {
            Assert(message != null);

            Message = message;
        }
    }
}