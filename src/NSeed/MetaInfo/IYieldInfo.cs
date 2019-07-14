namespace NSeed.MetaInfo
{
    /// <summary>
    /// Contains properties common for all yield meta info classes.
    /// </summary>
    public interface IYieldInfo
    {
        /// <summary>
        /// The <see cref="ISeed"/> that yields this yield.
        /// </summary>
        SeedInfo YieldingSeed { get;  }
    }
}