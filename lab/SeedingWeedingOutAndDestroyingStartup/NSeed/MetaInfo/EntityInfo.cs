using System;
using System.Collections.Generic;
using System.Linq;

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
        internal EntityInfo(object implementation, Type? type, string fullName, IReadOnlyCollection<Error> directErrors)
            : base(implementation, type, fullName, directErrors)
        {
        }

        /// <inheritdoc />
        protected override IEnumerable<MetaInfo> GetDirectChildMetaInfos()
        {
            return Enumerable.Empty<MetaInfo>();
        }
    }
}
