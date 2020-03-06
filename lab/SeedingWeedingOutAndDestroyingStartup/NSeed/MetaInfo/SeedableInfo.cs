using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes common meta info available in all seedable classes.
    /// A seedable class is a class that implements either <see cref="ISeed"/>
    /// or <see cref="IScenario"/>.
    /// </summary>
    public abstract class SeedableInfo : MetaInfo, IDescribedMetaInfo
    {
        /// <inheritdoc/>
        public string FriendlyName { get; }

        /// <inheritdoc/>
        public string Description { get; }

        /// <summary>
        /// Gets the <see cref="SeedBucket"/> that contains this seedable or null
        /// if the seedable is not contained in any seed bucket or if the
        /// seed bucket cannot be uniquely identified.
        /// </summary>
        // Set is called either by SeedBucketInfo constructor in case of contained seedables
        // or by the concrete ISeedBucketInfoBuilder in the case of non-contained seedables.
        public SeedBucketInfo? SeedBucket { get; internal set; }

        /// <summary>
        /// Gets seedables explicitly required by this seedable.
        /// <br/>
        /// A seedable is explicitely required if the implementation
        /// of this seedable has a <see cref="RequiresAttribute"/>
        /// whose <see cref="RequiresAttribute.SeedableType"/>
        /// is the implementation of the required seedable.
        /// </summary>
        public IReadOnlyCollection<SeedableInfo> ExplicitlyRequiredSeedables { get; }

        /// <summary>
        /// Gets all seedables required by this seedable.
        /// <br/>
        /// Returns union of <see cref="ExplicitlyRequiredSeedables"/> and
        /// other seedables that this seedable requires.
        /// For example, seeds can implicitly require other seeds
        /// by requiring their yields (see: <see cref="SeedInfo.RequiredYields"/>).
        /// </summary>
        public abstract IReadOnlyCollection<SeedableInfo> RequiredSeedables { get; }

        internal SeedableInfo(
            object implementation,
            Type? type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<SeedableInfo> explicitlyRequiredSeedables,
            IReadOnlyCollection<Error> directErrors)
            : base(implementation, type, fullName, directErrors)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(friendlyName));
            System.Diagnostics.Debug.Assert(explicitlyRequiredSeedables.All(required => required != null));

            FriendlyName = friendlyName;
            Description = description;
            ExplicitlyRequiredSeedables = explicitlyRequiredSeedables;
        }

        /// <summary>
        /// Returns <see cref="SeedableInfo"/>s required by <paramref name="seedableInfos"/> that are
        /// not already contained in <paramref name="seedableInfos"/>.
        /// </summary>
        internal static IReadOnlyCollection<SeedableInfo> GetRequiredSeedableInfosNotContainedIn(IEnumerable<SeedableInfo> seedableInfos)
        {
            var originalSeedableInfos = new HashSet<SeedableInfo>(seedableInfos);
            var result = new HashSet<SeedableInfo>();

            RecursivelyFindAllNonContainedSeedableInfos(seedableInfos);

            return result;

            void RecursivelyFindAllNonContainedSeedableInfos(IEnumerable<SeedableInfo> currentSeedableInfos)
            {
                foreach (var seedableInfo in currentSeedableInfos)
                {
                    if (!originalSeedableInfos.Contains(seedableInfo)) result.Add(seedableInfo);
                    RecursivelyFindAllNonContainedSeedableInfos(seedableInfo.RequiredSeedables);
                }
            }
        }
    }
}
