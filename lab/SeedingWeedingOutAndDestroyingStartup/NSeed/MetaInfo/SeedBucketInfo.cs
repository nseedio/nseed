using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes a single concrete <see cref="SeedBucket"/> implementation.
    /// </summary>
    public class SeedBucketInfo : MetaInfo, IDescribedMetaInfo
    {
        /// <inheritdoc/>
        public string FriendlyName { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <summary>
        /// Gets seedables (<see cref="ISeed"/>s and <see cref="IScenario"/>s) contained in this <see cref="SeedBucket"/>.
        /// <br/>
        /// To get only <see cref="ISeed"/>s or <see cref="IScenario"/>s call
        /// <see cref="ContainedSeedables"/>.OfType&lt;<see cref="SeedInfo"/>&gt; or
        /// <see cref="ContainedSeedables"/>.OfType&lt;<see cref="ScenarioInfo"/>&gt; respectively.
        /// </summary>
        /// <remarks>
        /// These seedables can require seedables from other <see cref="SeedBucket"/>s.
        /// TODO: Explain how to get those seedables.
        /// </remarks>
        public IReadOnlyCollection<SeedableInfo> ContainedSeedables { get; }

        /// <summary>
        /// Gets.
        /// TODO.
        /// </summary>
        public IReadOnlyCollection<SeedBucketStartupInfo> Startups { get; }

        internal SeedBucketInfo(
            object implementation,
            Type? type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<SeedableInfo> containedSeedables,
            IReadOnlyCollection<SeedBucketStartupInfo> startups,
            IReadOnlyCollection<Error> directErrors)
            : base(implementation, type, fullName, directErrors)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(friendlyName));
            System.Diagnostics.Debug.Assert(containedSeedables.All(seedable => seedable != null && seedable.SeedBucket == null));

            FriendlyName = friendlyName;
            Description = description;

            ContainedSeedables = containedSeedables;
            foreach (var seedable in containedSeedables) seedable.SeedBucket = this;

            Startups = startups;
            foreach (var startup in startups) startup.SeedBucket = this;
        }

        /// <inheritdoc />
        protected override IEnumerable<MetaInfo> GetDirectChildMetaInfos() => ContainedSeedables.Cast<MetaInfo>().Concat(Startups);
    }
}
