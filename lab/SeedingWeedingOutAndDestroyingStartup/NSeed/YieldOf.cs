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
        /// <remarks>
        /// This property is set by the NSeed seeding engine. Never set this property in your code.
        /// </remarks>
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        // CS8618:
        // This property will always be set via reflection during the seeding by the seeding engine.
        // SA1623:
        // Since only the getter can be called we do not want to have the standard "Gets and sets" text in the summary.
#pragma warning disable SA1623 // Property summary documentation should match accessors
        protected TSeed Seed { get; set; } // TODO: Check why setting the private setter via reflection does not work?! It works in EF Core ;-)
#pragma warning restore SA1623 // Property summary documentation should match accessors
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
    }
}
