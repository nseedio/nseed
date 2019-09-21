using System;
using System.Collections.Generic;
using System.Linq;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes a single concrete <see cref="IScenario"/> implementation.
    /// </summary>
    public sealed class ScenarioInfo : SeedableInfo
    {
        internal ScenarioInfo(
            object implementation,
            Type? type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<SeedableInfo> explicitlyRequiredSeedables,
            IReadOnlyCollection<Error> directErrors)
            : base(implementation, type, fullName, friendlyName, description, explicitlyRequiredSeedables, directErrors)
        {
        }

        /// <summary>
        /// Gets all seedables required by this scenario.
        /// <br/>
        /// Returns the <see cref="SeedableInfo.ExplicitlyRequiredSeedables"/> since
        /// scenarios can only explicitly require other seedables (through the
        /// <see cref="RequiresAttribute"/>).
        /// </summary>
        public override IEnumerable<SeedableInfo> RequiredSeedables => ExplicitlyRequiredSeedables;

        /// <inheritdoc />
        protected override IEnumerable<MetaInfo> GetDirectChildMetaInfos()
        {
            return Enumerable.Empty<MetaInfo>();
        }
    }
}
