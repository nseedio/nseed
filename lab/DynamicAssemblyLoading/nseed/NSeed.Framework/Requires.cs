using System;

namespace NSeed
{
    /// <summary>
    /// Specifies that a seed or scenario requires another seed or scenario.
    /// The seed or scenario on which this attribute is applied requires the seed or scenario which is defined by the attribute itself.
    /// This attribute should be applied only on seeds and scenarios
    /// (classes that implement <see cref="ISeed"/> or <see cref="IScenario"/>, respectively).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public sealed class RequiresAttribute : Attribute
    {
        /// <summary>
        /// Type of the seed or scenario that the seed or scenario on which the attribute is applied depends on.
        /// </summary>
        public Type SeedOrScenarioType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresAttribute"/> class that specifies that some seed or scenario
        /// requires the another seed or scenario of the type <paramref name="seedOrScenarioType"/>.
        /// </summary>
        public RequiresAttribute(Type seedOrScenarioType) => SeedOrScenarioType = seedOrScenarioType;
    }
}