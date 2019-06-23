using System;
using System.Collections.Generic;
using System.Linq;
using NSeed.Seeding.Extensions;
using static System.Diagnostics.Debug;

namespace NSeed.Seeding
{
    /// <summary>
    /// Represents the tree of <see cref="SeedInfo"/> instances.
    /// Each <see cref="SeedInfo"/> describes a single seed.
    /// The tree describes all the seeds found in the seeding and the dependency relation between them.
    /// </summary>
    public class SeedInfoTree
    {
        // Contains all the seeds in the seeding. Dictionary key is the seed type.
        private readonly Dictionary<Type, SeedInfo> allSeedInfos = new Dictionary<Type, SeedInfo>();

        /// <summary>
        /// All <see cref="SeedInfo"/> instances contained in the seeding.
        /// The <see cref="SeedInfo"/> instances are returned in no particular order.
        /// To obtain an ordered list of <see cref="SeedInfo"/> traverse the tree
        /// on your own starting from the <see cref="RootSeeds"/>.
        /// </summary>
        public IEnumerable<SeedInfo> AllSeeds => allSeedInfos.Values;

        /// <summary>
        /// The root <see cref="SeedInfo"/> instances, thus those seeds who do not depend on any other seeds.
        /// </summary>
        public IEnumerable<SeedInfo> RootSeeds => allSeedInfos.Values.Where(seedInfo => !seedInfo.DependsOn.Any());

        /// <summary>
        /// Creates a new instance of the <see cref="SeedInfoTree"/> that describes the seed build upon the <paramref name="seedTypes"/>.
        /// </summary>
        internal static SeedInfoTree Create(IEnumerable<Type> seedTypes)
        {
            Assert(seedTypes != null);
            Assert(seedTypes.All(type => type.IsSeedType()));

            SeedInfoTree result = new SeedInfoTree();

            foreach (var seedType in seedTypes)
                result.GetOrCreateSeedInfoFor(seedType);

            return result;
        }


        /// <summary>
        /// Gets <see cref="SeedInfo"/> that describes the <paramref name="seedType"/>.
        /// If such seed info does not exist it will be created and added to the tree.
        /// </summary>
        internal SeedInfo GetOrCreateSeedInfoFor(Type seedType)
        {
            Assert(seedType != null);
            Assert(seedType.IsSeedType());

            if (!allSeedInfos.ContainsKey(seedType))
                allSeedInfos.Add(seedType, SeedInfo.CreateFor(seedType, this));

            return allSeedInfos[seedType];
        }
    }
}