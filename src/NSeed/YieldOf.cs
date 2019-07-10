namespace NSeed
{
    /// <summary>
    /// Base class for all yield classes.
    /// <br/>
    /// A yield class is a classes named "Yield" that is nested within a concrete
    /// <see cref="ISeed"/> implementation whose yield it represents.
    /// </summary>
    /// <typeparam name="TSeed">Type of the <see cref="ISeed"/> that yields the yield represented by the concrete derived class.</typeparam>
    public abstract class YieldOf<TSeed> where TSeed : class, ISeed
    {
        /// <summary>
        /// The original <see cref="ISeed"/> that has yielded the yield represented by this yield class.
        /// </summary>
        protected TSeed Seed { get; set; }
    }
}