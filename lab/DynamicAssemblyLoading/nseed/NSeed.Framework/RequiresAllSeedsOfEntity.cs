using System;

namespace NSeed
{
    /// <summary>
    /// Specifies that a seed or scenario requires all seeds that create entities of a certain type.
    /// The seed or scenario on which this attribute is applied requires all seeds that create entities of the <see cref="EntityType"/>.
    /// This attribute should be applied only on seeds and scenarios
    /// (classes that implement <see cref="ISeed"/> or <see cref="IScenario"/>, respectively).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class RequiresAllSeedsOfEntityAttribute : Attribute
    {
        /// <summary>
        /// Type of the entity that the required seeds create.
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresAllSeedsOfEntityAttribute"/> class that specifies that some seed or scenario
        /// requires the all seeds that create entities of the type <paramref name="entityType"/>.
        /// </summary>
        public RequiresAllSeedsOfEntityAttribute(Type entityType) => EntityType = entityType;
    }
}