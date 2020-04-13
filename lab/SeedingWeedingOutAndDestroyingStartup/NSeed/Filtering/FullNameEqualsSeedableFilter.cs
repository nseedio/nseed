using NSeed.MetaInfo;
using System.Linq;

namespace NSeed.Filtering
{
    /// <summary>
    /// TODO.
    /// </summary>
    public class FullNameEqualsSeedableFilter : ISeedableFilter
    {
        private readonly string[] seedableFullNames;

        /// <summary>
        /// Initializes a new instance of the <see cref="FullNameEqualsSeedableFilter"/> class.
        /// TODO.
        /// </summary>
        /// <param name="seedableFullNames">TODO.</param>
        public FullNameEqualsSeedableFilter(params string[] seedableFullNames)
        {
            this.seedableFullNames = seedableFullNames;
        }

        /// <inheritdoc/>
        public bool Accepts(SeedableInfo seedableInfo) => seedableFullNames.Any(fullName => seedableInfo.FullName == fullName);
    }
}
