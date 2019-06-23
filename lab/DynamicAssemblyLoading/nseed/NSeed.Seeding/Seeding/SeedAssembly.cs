using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSeed.Seeding.Extensions;
using static System.Diagnostics.Debug;

namespace NSeed.Seeding
{
    /// <summary>
    /// Describes an <see cref="Assembly"/> that contains the seeds that will be seeded.
    /// </summary>
    internal class SeedAssembly
    {
        public Assembly Assembly { get; }
        public Type SeedingSetupType { get; }
        public IEnumerable<Type> SeedTypes { get; }

        private SeedAssembly(Assembly assembly, Type seedingSetupType, IEnumerable<Type> seedTypes)
        {
            Assert(assembly != null);            
            if (seedingSetupType != null) Assert(seedingSetupType.IsSeedingSetupType());
            Assert(seedTypes != null);

            Assembly = assembly;
            SeedingSetupType = seedingSetupType;
            SeedTypes = seedTypes;
        }

        public static SeedAssembly Create(Assembly assembly, IEnumerable<string> seedFilters, Type seedingSetupType = null)
        {
            Assert(assembly != null);
            Assert(seedFilters != null);

            var seedingSetup = seedingSetupType ?? assembly
                .GetTypes()
                .FirstOrDefault(type => type.IsSeedingSetupType());

            var seedTypes = assembly.GetTypes().Where(type => type.IsSeedType());

            if (seedFilters.Any())
            {
                seedTypes = seedTypes.Where(type => seedFilters.Any(filter => type.FullName.Contains(filter, StringComparison.InvariantCultureIgnoreCase)));
            }

            return new SeedAssembly(assembly, seedingSetup, seedTypes);
        }
    }
}