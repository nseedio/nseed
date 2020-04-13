using NSeed.MetaInfo;

namespace NSeed.Filtering
{
    // TODO: Should this interface be in Abstractions? Where to put it? Where to put the implementations?

    /// <summary>
    /// TODO.
    /// </summary>
    public interface ISeedableFilter
    {
        /// <summary>
        /// TODO.
        /// </summary>
        /// <param name="seedableInfo">TODO2.</param>
        /// <returns>TODO. TODO.</returns>
        bool Accepts(SeedableInfo seedableInfo);
    }
}
