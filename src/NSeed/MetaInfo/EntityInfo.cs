using System;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes a single entity seeded by one or more concrete <see cref="ISeed"/>s.
    /// </summary>
    /// <remarks>
    /// In NSeed terminology seeds seed entity instances.
    /// </remarks>
    public sealed class EntityInfo : MetaInfo
    {
        internal EntityInfo(Type type, string fullName)
            :base(type, fullName)
        {            
        }
    }
}