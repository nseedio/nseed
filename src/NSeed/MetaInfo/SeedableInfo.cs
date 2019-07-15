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
        /// Seedables explicitly required by this seedable.
        /// <br/>
        /// A seedable is explicitely required if the implementation
        /// of this seedable has a <see cref="RequiresAttribute"/>
        /// whose <see cref="RequiresAttribute.SeedableType"/>
        /// is the implementation of the required seedable.
        /// </summary>
        public IReadOnlyCollection<SeedableInfo> ExplicitlyRequiredSeedables { get; }

        /// <summary>
        /// All seedables required by this seedable.
        /// <br/>
        /// Returns union of <see cref="ExplicitlyRequiredSeedables"/> and
        /// other seedables that this seedable requires.
        /// For example, seeds can implicitly require other seeds
        /// by requiring their yields (see: <see cref="SeedInfo.RequiredYields"/>).
        /// </summary>
        public abstract IEnumerable<SeedableInfo> RequiredSeedables { get; }

        internal SeedableInfo(
            Type type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<SeedableInfo> explicitlyRequiredSeedables)
            :base(type, fullName)
        {            
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(friendlyName));
            System.Diagnostics.Debug.Assert(description != null);
            System.Diagnostics.Debug.Assert(explicitlyRequiredSeedables != null);
            System.Diagnostics.Debug.Assert(explicitlyRequiredSeedables.All(required => required != null));

            FriendlyName = friendlyName;
            Description = description;
            ExplicitlyRequiredSeedables = explicitlyRequiredSeedables;
        }
    }
}