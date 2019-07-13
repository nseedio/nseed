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
            IReadOnlyCollection<SeedableInfo> explicitlyRequires)
            :base(type, fullName, friendlyName, description, explicitlyRequires)
        {            
        }

        /// <summary>
        /// All seedables required by this scenario.
        /// <br/>
        /// Returns the <see cref="SeedableInfo.ExplicitlyRequires"/> since
        /// scenarios can require other seedables only through the
        /// <see cref="RequiresAttribute"/>.
        /// </summary>
        public override IEnumerable<SeedableInfo> Requires => ExplicitlyRequires;
    }
}