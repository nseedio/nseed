using System.Threading.Tasks;

namespace NSeed
{
    // ReSharper disable UnusedTypeParameter
    /// <summary>
    /// A single seed that seeds three types of entities.
    /// </summary>
    public interface ISeed<TEntity1, TEntity2, TEntity3> : ISeed { }
    /// <summary>
    /// A single seed that seeds two types of entities.
    /// </summary>
    public interface ISeed<TEntity1, TEntity2> : ISeed { }
    /// <summary>
    /// A single seed that seeds entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    public interface ISeed<TEntity> : ISeed { }
    // ReSharper restore UnusedTypeParameter

    /// <summary>
    /// A single seed.
    /// </summary>
    /// <remarks>
    /// To enable better categorization of seeds, it is advisable to use
    /// one of the generic versions of the <see cref="ISeed"/> interface
    /// that denote the type of entities that the seed seeds.
    /// Ideally, a seed should seed entities of a single type.
    /// Thus, the mostly used <see cref="ISeed"/> interface in your application
    /// should be <see cref="ISeed{TEntity}"/>.
    /// </remarks>
    public interface ISeed
    {
        Task Seed();
        Task<bool> OutputAlreadyExists();
        Task UnSeed();
    }
}
