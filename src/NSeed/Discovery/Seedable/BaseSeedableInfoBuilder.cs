using System;
using System.Collections.Generic;
using NSeed.Guards;
using NSeed.MetaInfo;

namespace NSeed.Discovery.Seedable
{
    // TODO-IG: See if we should have only one generic type parameter.
    //          For the moment it seams like all of the parameters will be of the same type.
    //          For reflection based discovery, it will be System.Type.
    //          For source code based discovery most likely always SyntaxNode.
    internal abstract class BaseSeedableInfoBuilder<TSeedableImplementation, TEntityImplementation> : ISeedableInfoBuilder<TSeedableImplementation>
        where TSeedableImplementation : class
        where TEntityImplementation : class
    {
        private readonly ISeedableTypeExtractor<TSeedableImplementation> typeExtractor;
        private readonly ISeedableFullNameExtractor<TSeedableImplementation> fullNameExtractor;
        private readonly ISeedableFriendlyNameExtractor<TSeedableImplementation> friendlyNameExtractor;
        private readonly ISeedableDescriptionExtractor<TSeedableImplementation> descriptionExtractor;
        private readonly ISeedEntitiesExtractor<TSeedableImplementation> entitiesExtractor;
        private readonly IExplicitlyRequiredSeedablesExtractor<TSeedableImplementation> explicitlyRequiredSeedablesExtractor;
        private readonly IMetaInfoPool<TSeedableImplementation, SeedableInfo> seedableInfoPool;

        // Keeps track of the current build chain in order to ignore circular dependencies.
        private readonly Stack<TSeedableImplementation> buildChain = new Stack<TSeedableImplementation>();

        internal BaseSeedableInfoBuilder(ISeedableTypeExtractor<TSeedableImplementation> typeExtractor,
                                     ISeedableFullNameExtractor<TSeedableImplementation> fullNameExtractor,
                                     ISeedableFriendlyNameExtractor<TSeedableImplementation> friendlyNameExtractor,
                                     ISeedableDescriptionExtractor<TSeedableImplementation> descriptionExtractor,
                                     ISeedEntitiesExtractor<TSeedableImplementation> entitiesExtractor,
                                     Func<ISeedableInfoBuilder<TSeedableImplementation>, IExplicitlyRequiredSeedablesExtractor<TSeedableImplementation>> explicitlyRequiredSeedablesExtractorFactory,
                                     IMetaInfoPool<TSeedableImplementation, SeedableInfo> seedableInfoPool)
        {
            typeExtractor.MustNotBeNull(nameof(typeExtractor));
            fullNameExtractor.MustNotBeNull(nameof(fullNameExtractor));
            friendlyNameExtractor.MustNotBeNull(nameof(friendlyNameExtractor));
            descriptionExtractor.MustNotBeNull(nameof(descriptionExtractor));
            entitiesExtractor.MustNotBeNull(nameof(entitiesExtractor));
            explicitlyRequiredSeedablesExtractorFactory.MustNotBeNull(nameof(explicitlyRequiredSeedablesExtractorFactory));
            seedableInfoPool.MustNotBeNull(nameof(seedableInfoPool));

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.friendlyNameExtractor = friendlyNameExtractor;
            this.descriptionExtractor = descriptionExtractor;
            this.entitiesExtractor = entitiesExtractor;
            explicitlyRequiredSeedablesExtractor = explicitlyRequiredSeedablesExtractorFactory(this);
            this.seedableInfoPool = seedableInfoPool;
        }

        SeedableInfo IMetaInfoBuilder<TSeedableImplementation, SeedableInfo>.BuildFrom(TSeedableImplementation implementation)
        {
            System.Diagnostics.Debug.Assert(implementation != null);

            if (buildChain.Contains(implementation)) return null;

            buildChain.Push(implementation);
            var result = seedableInfoPool.GetOrAdd(implementation, CreateSeedableInfo);
            buildChain.Pop();

            return result;
        }

        private SeedableInfo CreateSeedableInfo(TSeedableImplementation implementation)
        {
            var errorCollector = new DistinctErrorCollectorAndProvider();

            // A "Seedable" is actually a dicriminated union of ISeed and IScenario.
            // The way we distinguis between them everywhere is just a workaround
            // for non-existing discriminated unions in C#.

            bool isSeedImplementation = IsSeedImplemenation(implementation);

            Type type = typeExtractor.ExtractFrom(implementation, errorCollector);
            string fullName = fullNameExtractor.ExtractFrom(implementation, errorCollector);
            string friendlyName = friendlyNameExtractor.ExtractFrom(implementation, errorCollector);
            string description = descriptionExtractor.ExtractFrom(implementation, errorCollector);
            var explicitelyRequires = explicitlyRequiredSeedablesExtractor.ExtractFrom(implementation, errorCollector);

            return isSeedImplementation
                ? (SeedableInfo)new SeedInfo
                  (
                      type,
                      fullName,
                      friendlyName,
                      description,
                      explicitelyRequires,
                      Array.Empty<SeedInfo>(), // TODO-IG: Add implicitly requires.
                      entitiesExtractor.ExtractFrom(implementation, errorCollector)
                  )
                : new ScenarioInfo
                 (
                      type,
                      fullName,
                      friendlyName,
                      description,
                      explicitelyRequires
                  );
        }

        protected internal abstract bool IsSeedImplemenation(TSeedableImplementation implementation);
    }
}