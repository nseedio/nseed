using NSeed.MetaInfo;

namespace NSeed.Filtering
{
    /// <summary>
    /// TODO.
    /// </summary>
    public class AcceptAllSeedableFilter : ISeedableFilter
    {
        /// <summary>
        /// TODO.
        /// </summary>
        public static readonly AcceptAllSeedableFilter Instance = new AcceptAllSeedableFilter();

        private AcceptAllSeedableFilter() { }

        /// <inheritdoc/>
        public bool Accepts(SeedableInfo seedableInfo) => true;
    }
}
