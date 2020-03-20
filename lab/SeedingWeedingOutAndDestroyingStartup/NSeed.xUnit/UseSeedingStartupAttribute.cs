using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Seeding;
using System;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NSeed.Xunit
{
    /// <summary>
    /// TODO.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public sealed class UseSeedingStartupAttribute : Attribute
    {
        public Type SeedingStartupType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UseSeedingStartupAttribute"/> class.
        /// </summary>
        /// <param name="seedingStartupType">TODO.</param>
        public UseSeedingStartupAttribute(Type seedingStartupType)
        {
            SeedingStartupType = seedingStartupType;
        }
    }
}
