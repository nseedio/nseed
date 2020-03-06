using System.Threading.Tasks;

namespace NSeed
{
    // All these three interfaces are actually the "same" interface.
    // That's why we want to have them in the same file.
    // The purpose of the generic parameters is explained in the interface description.
    // There is no need to additionally document the parameters.
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1618 // Generic type parameters should be documented
    /// <summary>
    /// A <see cref="ISeed"/> that yields three types of entities.
    /// </summary>
    public interface ISeed<TEntity1, TEntity2, TEntity3> : ISeed { }

    /// <summary>
    /// A <see cref="ISeed"/> that yields two types of entities.
    /// </summary>
    public interface ISeed<TEntity1, TEntity2> : ISeed { }

    /// <summary>
    /// A <see cref="ISeed"/> that yields entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    public interface ISeed<TEntity> : ISeed { }
#pragma warning restore SA1618 // Generic type parameters should be documented
#pragma warning restore SA1402 // File may only contain a single type

    /// <summary>
    /// A single seed.
    /// </summary>
    /// <remarks>
    /// To enable better categorization of seeds, the best practice is to use
    /// one of the generic versions of the <see cref="ISeed"/> interface
    /// that denote the type of entities that the seed yields.
    /// <br/>
    /// Ideally, a seed should yield entities of a single type.
    /// Thus, the mostly used <see cref="ISeed"/> interface in your application
    /// should be <see cref="ISeed{TEntity}"/>.
    /// </remarks>
    public interface ISeed
    {
        /// <summary>
        /// Seeds this seed and produces its yield.
        /// </summary>
        Task Seed();

        /// <summary>
        /// Returns true if the yield of this seed already exists.
        /// This means that the <see cref="Seed"/> method was already successfuly executed.
        /// </summary>
        Task<bool> HasAlreadyYielded();
    }
}
