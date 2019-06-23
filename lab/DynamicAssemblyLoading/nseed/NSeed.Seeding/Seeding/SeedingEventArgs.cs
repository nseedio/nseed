using System;
using static System.Diagnostics.Debug;

namespace NSeed.Seeding
{
    public class SeedingEventArgs : EventArgs
    {
        /// <summary>
        /// The current seeding step in the seeding, starting from 1.
        /// If this property is zero, that means that the seeding didn't start when the
        /// event was sent. In that case, <see cref="Seed"/> will be null.
        /// </summary>
        public int SeedingStep { get; }

        /// <summary>
        /// The current seed in the seeding.
        /// The seed can be null if the event is sent before the seeding starts (e.g. an error before the seeding starts).
        /// In that case the <see cref="SeedingStep"/> will be zero because the seeding didn't start.
        /// </summary>
        public SeedInfo Seed { get; }

        public SeedingEventArgs(int seedingStep, SeedInfo seed)
        {
            Assert(seedingStep >= 0);
            Assert((seedingStep > 0 && seed != null) || (seedingStep == 0 && seed == null));

            SeedingStep = seedingStep;
            Seed = seed;
        }
    }
}