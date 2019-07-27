using System.Reflection;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes a yield class seeded by the <see cref="YieldingSeed"/> and required by the <see cref="RequiringSeed"/>.
    /// <br/>
    /// A yield of some <see cref="ISeed"/> is used by another <see cref="ISeed"/>
    /// if the another seed has exactly one yield access property of the
    /// type of that yield.
    /// </summary>
    public sealed class RequiredYieldInfo : MetaInfo, IYieldInfo
    {
        /// <inheritdoc/>
        public SeedInfo YieldingSeed { get; }

        /// <summary>
        /// Gets the <see cref="ISeed"/> that requires this yield.
        /// </summary>
        public SeedInfo RequiringSeed { get; internal set; } // Set is called by the SeedInfo of the requiring seed.

        /// <summary>
        /// Gets the yield access property that provides access to the
        /// yield if such <see cref="PropertyInfo"/> exists; otherwise null.
        /// </summary>
        public PropertyInfo? YieldAccessProperty { get; }

        /// <summary>
        /// Gets the name of the yield access property.
        /// </summary>
        /// <remarks>
        /// If the <see cref="YieldAccessProperty"/> exists, this property is equal to its <see cref="MemberInfo.Name"/>.
        /// </remarks>
        public string YieldAccessPropertyName { get; }

        // CS8618:
        // The RequiringSeed property will be set later by the SeedInfo of the requiring seed.
#pragma warning disable CS8618 // Non-nullable field is uninitialized.
        internal RequiredYieldInfo(SeedInfo yieldingSeed, PropertyInfo? yieldAccessProperty, string yieldAccessPropertyName)
#pragma warning restore CS8618 // Non-nullable field is uninitialized.
            : base(
                  yieldingSeed.Yield!.Implementation, // The yielding seed, by definition, must have yield, therefore, safe "!".
                  yieldingSeed.Yield.Type,
                  yieldingSeed.Yield.FullName)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(yieldAccessPropertyName));

            YieldingSeed = yieldingSeed;
            YieldAccessProperty = yieldAccessProperty;
            YieldAccessPropertyName = yieldAccessPropertyName;
        }
    }
}
