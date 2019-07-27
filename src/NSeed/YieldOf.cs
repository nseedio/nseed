namespace NSeed
{
    /// <summary>
    /// Base class for all yield classes.
    /// <br/>
    /// A yield class is a classes named "Yield" that is nested within a concrete
    /// <see cref="ISeed"/> implementation whose yield it represents.
    /// </summary>
    /// <typeparam name="TSeed">Type of the <see cref="ISeed"/> that yields the yield represented by the concrete derived class.</typeparam>
    public abstract class YieldOf<TSeed>
        where TSeed : class, ISeed
    {
        /// <summary>
        /// Gets the original <see cref="ISeed"/> that has yielded the yield represented by this yield class.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        // CS8618:
        // This property will always be set via reflection during the seeding by the seeding engine.
        protected TSeed Seed { get; private set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
    }
}
