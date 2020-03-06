using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.MetaInfo
{
    // TODO: Add for what processing it applies: Seeding, WeedingOut, Destroying.

    /// <summary>
    /// TODO.
    /// </summary>
    public class SeedBucketStartupInfo : MetaInfo, IDescribedMetaInfo
    {
        /// <inheritdoc/>
        public string FriendlyName { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <summary>
        /// Gets the <see cref="SeedBucket"/> that contains this seed bucket startup or null
        /// if the seed bucket startup is not contained in any seed bucket or if the
        /// seed bucket cannot be uniquely identified.
        /// </summary>
        // Set is called by SeedBucketInfo constructor.
        public SeedBucketInfo? SeedBucket { get; internal set; }

        internal SeedBucketStartupInfo(
            object implementation,
            Type? type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<Error> directErrors)
            : base(implementation, type, fullName, directErrors)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(friendlyName));

            FriendlyName = friendlyName;
            Description = description;
        }

        /// <inheritdoc />
        protected override IEnumerable<MetaInfo> GetDirectChildMetaInfos()
        {
            return Enumerable.Empty<MetaInfo>();
        }
    }
}
