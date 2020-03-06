namespace NSeed.MetaInfo
{
    /// <summary>
    /// Contains properties common for all yield meta infos.
    /// </summary>
    public interface IYieldInfo
    {
        /// <summary>
        /// Gets the <see cref="ISeed"/> that yields this yield.
        /// </summary>
        SeedInfo YieldingSeed { get; }
    }
}
