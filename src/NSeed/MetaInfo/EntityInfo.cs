using System;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes the type of a single entity seeded by one or more concrete <see cref="ISeed"/>s.
    /// </summary>
    /// <remarks>
    /// In NSeed terminology seeds yield entities.
    /// </remarks>
    public sealed class EntityInfo : MetaInfo
    {
        internal EntityInfo(object implementation, Type type, string fullName)
            : base(implementation, type, fullName)
        {
        }
    }
}
