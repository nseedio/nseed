using System;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes a yield class seeded by the <see cref="YieldingSeed"/>.
    /// <br/>
    /// A yield class is a class nested within a seed class.
    /// Its name is always "Yield" and it derives from <see cref="YieldOf{TSeed}"/>
    /// where the TSeed is the type of its declaring seed class.
    /// </summary>
    public sealed class ProvidedYieldInfo : MetaInfo, IYieldInfo
    {
        /// <inheritdoc/>
        public SeedInfo YieldingSeed { get; internal set; } // Set is called by the SeedInfo of the yielding seed.

        internal ProvidedYieldInfo(object implementation, Type type, string fullName)
            : base(implementation, type, fullName)
        {
        }
    }
}
