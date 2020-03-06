using System;
using System.Collections.Generic;
using System.Linq;

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

        // CS8618:
        // The YieldingSeed property will be set later by the SeedInfo of the yielding seed.
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        internal ProvidedYieldInfo(object implementation, Type? type, string fullName, IReadOnlyCollection<Error> directErrors)
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
            : base(implementation, type, fullName, directErrors)
        {
        }

        /// <inheritdoc />
        protected override IEnumerable<MetaInfo> GetDirectChildMetaInfos()
        {
            return Enumerable.Empty<MetaInfo>();
        }
    }
}
