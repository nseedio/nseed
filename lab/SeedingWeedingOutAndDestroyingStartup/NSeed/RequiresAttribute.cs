using System;

namespace NSeed
{
    /// <summary>
    /// Specifies that a seedable class (a class that implements <see cref="ISeed"/> ot <see cref="IScenario"/>)
    /// requires yield of another seedable class.
    /// <br/>
    /// The seedable class annotated with this attribute requires yield of the <see cref="ISeed"/>
    /// or <see cref="IScenario"/> of the type <see cref="SeedableType"/>.
    /// </summary>
    /// <remarks>
    /// This attribute should only be applied on seedable classes
    /// (classes that implement <see cref="ISeed"/> or <see cref="IScenario"/>).
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class RequiresAttribute : Attribute
    {
        /// <summary>
        /// Gets the <see cref="Type"/> of a single <see cref="ISeed"/> or <see cref="IScenario"/>
        /// whose yield is required by the seedable class annotated with this attribute.
        /// </summary>
        public Type SeedableType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresAttribute"/> class that specifies that a seedable class
        /// requires yield of an <see cref="ISeed"/> or <see cref="IScenario"/> of the type <paramref name="seedableType"/>.
        /// </summary>
        /// <param name="seedableType">The type of the required <see cref="ISeed"/> or <see cref="IScenario"/>.</param>
        public RequiresAttribute(Type seedableType) => SeedableType = seedableType;
    }
}
