using NSeed.Extensions;
using NSeed.MetaInfo;
using System.Linq;

namespace NSeed.Filtering
{
    /// <summary>
    /// TODO.
    /// </summary>
    public class FullNameContainsSeedableFilter : ISeedableFilter
    {
        private readonly string[] partsOfSeedableFullName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FullNameContainsSeedableFilter"/> class.
        /// TODO.
        /// </summary>
        /// <param name="partsOfSeedableFullName">TODO.</param>
        public FullNameContainsSeedableFilter(params string[] partsOfSeedableFullName)
        {
            this.partsOfSeedableFullName = partsOfSeedableFullName;
        }

        /// <inheritdoc/>
        public bool Accepts(SeedableInfo seedableInfo) => partsOfSeedableFullName.Any(part => seedableInfo.FullName.Contains(part, System.StringComparison.InvariantCultureIgnoreCase));
    }
}
