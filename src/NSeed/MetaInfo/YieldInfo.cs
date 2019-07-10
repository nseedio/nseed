using System;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes the yield seeded by the <see cref="YieldInfo.Seed"/>.
    /// </summary>
    public sealed class YieldInfo : MetaInfo
    {
        /// <summary>
        /// The <see cref="ISeed"/> that yields this yield.
        /// </summary>
        public SeedInfo Seed { get; internal set; } // Set is called by the SeedInfo.

        internal YieldInfo(Type type, string fullName)
            :base(type, fullName)
        {            
        }
    }
}