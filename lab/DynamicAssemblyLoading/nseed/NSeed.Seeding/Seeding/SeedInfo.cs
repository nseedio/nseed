using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NSeed.Seeding.Extensions;

namespace NSeed.Seeding
{
    /// <summary>
    /// Describes a single seed.
    /// </summary>
    public class SeedInfo
    {
        /// <summary>
        /// Gets the <see cref="Type"/> of the seed described by this <see cref="SeedInfo"/>.
        /// </summary>
        public Type SeedType { get; }

        /// <summary>
        /// Gets seed output properties declared in the seed described by this <see cref="SeedInfo"/>.
        /// </summary>
        public IEnumerable<PropertyInfo> SeedOutputProperties { get; }

        /// <summary>
        /// Gets the <see cref="SeedInfo"/>s of the seeds that this seed depends on.
        /// </summary>
        public IEnumerable<SeedInfo> DependsOn { get; }

        /// <summary>
        /// Gets the seed display name.
        /// </summary>
        public string DisplayName => SeedType.Name.Replace('_', ' ');

        private SeedInfo(Type seedType, IEnumerable<PropertyInfo> seedOutputProperties, IEnumerable<SeedInfo> dependsOn)
        {
            Debug.Assert(seedType != null);
            Debug.Assert(seedType.IsSeedType());
            Debug.Assert(seedOutputProperties != null);
            Debug.Assert(seedOutputProperties.All(property => property.IsSeedOutputProperty()));
            Debug.Assert(dependsOn != null);

            SeedType = seedType;
            SeedOutputProperties = seedOutputProperties;
            DependsOn = dependsOn;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeedInfo"/> that describes the seed
        /// represented by the <paramref name="seedType"/>.
        /// </summary>
        internal static SeedInfo CreateFor(Type seedType, SeedInfoTree seedInfoTree)
        {
            Debug.Assert(seedType != null);
            Debug.Assert(seedType.IsSeedType());
            Debug.Assert(seedInfoTree != null);

            var seedOutputProperties = GetSeedOutputProperty();

            // TODO: Check that we do not have any circular dependencies.
            // TODO: Check all other things as well :-)

            return new SeedInfo(seedType, seedOutputProperties, GetExplicitDependencies().Concat(GetImplicitDependencies()).ToList());

            PropertyInfo[] GetSeedOutputProperty()
            {
                return seedType
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(property => property.IsSeedOutputProperty())
                    .ToArray();
            }

            IEnumerable<SeedInfo> GetImplicitDependencies()
            {
                return seedOutputProperties
                    .Select(property => seedInfoTree.GetOrCreateSeedInfoFor(property.PropertyType.DeclaringType));
            }

            IEnumerable<SeedInfo> GetExplicitDependencies()
            {
                // TODO: Add checks: SeedType != null. SeedType.IsSeedType().

                return seedType
                    .GetCustomAttributes(typeof(RequiresAttribute), false)
                    .Cast<RequiresAttribute>()
                    .Select(attribute => attribute.SeedOrScenarioType)
                    .Select(seedInfoTree.GetOrCreateSeedInfoFor);
            }
        }
    }
}