using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace NSeed
{
    // ReSharper disable UnusedTypeParameter
    /// <summary>
    /// A <see cref="ISeed"/> that seeds three types of entities.
    /// </summary>
    public interface ISeed<TEntity1, TEntity2, TEntity3> : ISeed { }

    /// <summary>
    /// A <see cref="ISeed"/> that seeds two types of entities.
    /// </summary>
    public interface ISeed<TEntity1, TEntity2> : ISeed { }

    /// <summary>
    /// A <see cref="ISeed"/> that seeds entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    public interface ISeed<TEntity> : ISeed { }
    // ReSharper restore UnusedTypeParameter

    /// <summary>
    /// A single seed.
    /// </summary>
    /// <remarks>
    /// To enable better categorization of seeds, the best practice is to use
    /// one of the generic versions of the <see cref="ISeed"/> interface
    /// that denote the type of entities that the seed seeds.
    /// <br/>
    /// Ideally, a seed should seed entities of a single type.
    /// Thus, the mostly used <see cref="ISeed"/> interface in your application
    /// should be <see cref="ISeed{TEntity}"/>.
    /// </remarks>
    public interface ISeed
    {
        /// <summary>
        /// Seeds this seed.
        /// </summary>
        Task Seed();

        /// <summary>
        /// Returns true if the yield of this seed exists.
        /// This means that the <see cref="Seed"/> method was already successfuly executed.
        /// </summary>
        Task<bool> HasAlreadyYielded();
    }

    internal static class SeedInterfaceTypeExtensions
    {
        private static readonly HashSet<Type> SeedInterfacesWithEntities = new HashSet<Type>
        {
            typeof(ISeed<>),
            typeof(ISeed<,>),
            typeof(ISeed<,,>)
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsSeedInterfaceWithEntities(this Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);

            return SeedInterfacesWithEntities.Contains(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSeedType(this Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);

            return type.GetInterfaces().Contains(typeof(ISeed));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSeedableType(this Type type)
        {
            System.Diagnostics.Debug.Assert(type != null);

            return type.GetInterfaces().Any(@interface => @interface == typeof(ISeed) || @interface == typeof(IScenario));
        }
    }
}