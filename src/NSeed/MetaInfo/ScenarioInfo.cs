using System;
using System.Collections.Generic;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes a single concrete <see cref="IScenario"/> implementation.
    /// </summary>
    public sealed class ScenarioInfo : SeedableInfo
    {
        internal ScenarioInfo(
            Type type,
            string fullName,
            string friendlyName,
            string description,
            IReadOnlyCollection<SeedableInfo> explicitlyRequiredSeedables)
            :base(type, fullName, friendlyName, description, explicitlyRequiredSeedables)
        {            
        }

        /// <summary>
        /// All seedables required by this scenario.
        /// <br/>
        /// Returns the <see cref="SeedableInfo.ExplicitlyRequiredSeedables"/> since
        /// scenarios can only explicitly require other seedables (through the
        /// <see cref="RequiresAttribute"/>).
        /// </summary>
        public override IEnumerable<SeedableInfo> RequiredSeedables => ExplicitlyRequiredSeedables;
    }
}