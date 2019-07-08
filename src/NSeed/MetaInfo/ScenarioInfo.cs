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
            :base(type, fullName, friendlyName, description, explicitlyRequires, Array.Empty<SeedInfo>())
        {            
        }
    }
}