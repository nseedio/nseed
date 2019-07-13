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
    public abstract class SeedableInfo : MetaInfo
    {
        /// <summary>
        /// The friendly name of the seedable.
        /// </summary>
        /// <returns>
        /// If the seedable implementation is annotated with the <see cref="FriendlyNameAttribute"/>
        /// the <see cref="FriendlyNameAttribute.FriendlyName"/> is returned.
        /// <br/>
        /// Otherwise, the humanized version of the implementation type name is returned.
        /// </returns>
        public string FriendlyName { get; }

        /// <summary>
        /// The description of the seedable.
        /// </summary>
        /// <returns>
        /// If the seedable implementation is annotated with the <see cref="DescriptionAttribute"/>
        /// the <see cref="DescriptionAttribute.Description"/> is returned.
        /// <br/>
        /// Otherwise, the <see cref="string.Empty"/> is returned.
        /// </returns>
        public string Description { get; }

        /// <summary>
        /// Seedables explicitly required by this seedable.
        /// <br/>
        /// A seedable is explicitely required if the implementation
        /// of this seedable has a <see cref="RequiresAttribute"/>
        /// whose <see cref="RequiresAttribute.SeedableType"/>
        /// is the implementation of the required seedable.
        /// </summary>
        public IReadOnlyCollection<SeedableInfo> ExplicitlyRequires { get; }

        /// <summary>
        /// All seedables required by this seedable.
        /// <br/>
        /// Returns union of all <see cref="ExplicitlyRequires"/> seedables and
        /// other seedables that this seedable requires.
        /// For example, seeds require other seeds
        /// by using they yields (see: <see cref="SeedInfo.UsesYields"/>).
        /// </summary>
        public abstract IEnumerable<SeedableInfo> Requires { get; }

        internal SeedableInfo(
            Type type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<SeedableInfo> explicitlyRequires)
            :base(type, fullName)
        {            
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(friendlyName));
            System.Diagnostics.Debug.Assert(description != null);
            System.Diagnostics.Debug.Assert(explicitlyRequires != null);
            System.Diagnostics.Debug.Assert(explicitlyRequires.All(required => required != null));

            FriendlyName = friendlyName;
            Description = description;
            ExplicitlyRequires = explicitlyRequires;
        }
    }
}