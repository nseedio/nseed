using NSeed.MetaInfo;
using System;
using System.Collections.Generic;

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
        private readonly ITypeExtractor<TSeedableImplementation> typeExtractor;
        private readonly IFullNameExtractor<TSeedableImplementation> fullNameExtractor;
        private readonly IFriendlyNameExtractor<TSeedableImplementation> friendlyNameExtractor;
        private readonly IDescriptionExtractor<TSeedableImplementation> descriptionExtractor;
        private readonly ISeedEntitiesExtractor<TSeedableImplementation> entitiesExtractor;
        private readonly ISeedProvidedYieldExtractor<TSeedableImplementation> providedYieldExtractor;
        private readonly IExplicitlyRequiredSeedablesExtractor<TSeedableImplementation> explicitlyRequiredSeedablesExtractor;
        private readonly ISeedRequiredYieldsExtractor<TSeedableImplementation> requiredYieldsExtractor;
        private readonly IMetaInfoPool<TSeedableImplementation, SeedableInfo> seedableInfoPool;

        // Keeps track of the current build chain in order to ignore circular dependencies.
        private readonly Stack<TSeedableImplementation> buildChain = new Stack<TSeedableImplementation>();

        internal BaseSeedableInfoBuilder(
            ITypeExtractor<TSeedableImplementation> typeExtractor,
            IFullNameExtractor<TSeedableImplementation> fullNameExtractor,
            IFriendlyNameExtractor<TSeedableImplementation> friendlyNameExtractor,
            IDescriptionExtractor<TSeedableImplementation> descriptionExtractor,
            ISeedEntitiesExtractor<TSeedableImplementation> entitiesExtractor,
            ISeedProvidedYieldExtractor<TSeedableImplementation> providedYieldExtractor,
            Func<ISeedableInfoBuilder<TSeedableImplementation>, IExplicitlyRequiredSeedablesExtractor<TSeedableImplementation>> explicitlyRequiredSeedablesExtractorFactory,
            Func<ISeedableInfoBuilder<TSeedableImplementation>, ISeedRequiredYieldsExtractor<TSeedableImplementation>> requiredYieldsExtractorFactory,
            IMetaInfoPool<TSeedableImplementation, SeedableInfo> seedableInfoPool)
        {
            System.Diagnostics.Debug.Assert(typeExtractor != null);
            System.Diagnostics.Debug.Assert(fullNameExtractor != null);
            System.Diagnostics.Debug.Assert(friendlyNameExtractor != null);
            System.Diagnostics.Debug.Assert(descriptionExtractor != null);
            System.Diagnostics.Debug.Assert(entitiesExtractor != null);
            System.Diagnostics.Debug.Assert(providedYieldExtractor != null);
            System.Diagnostics.Debug.Assert(explicitlyRequiredSeedablesExtractorFactory != null);
            System.Diagnostics.Debug.Assert(requiredYieldsExtractorFactory != null);
            System.Diagnostics.Debug.Assert(seedableInfoPool != null);

            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.friendlyNameExtractor = friendlyNameExtractor;
            this.descriptionExtractor = descriptionExtractor;
            this.entitiesExtractor = entitiesExtractor;
            this.providedYieldExtractor = providedYieldExtractor;
            explicitlyRequiredSeedablesExtractor = explicitlyRequiredSeedablesExtractorFactory(this);
            requiredYieldsExtractor = requiredYieldsExtractorFactory(this);
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

        protected internal abstract bool IsSeedImplemenation(TSeedableImplementation implementation);

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
                      type,
                      fullName,
                      friendlyName,
                      description,
                      explicitelyRequires,
                      entitiesExtractor.ExtractFrom(implementation, errorCollector),
                      providedYieldExtractor.ExtractFrom(implementation, errorCollector),
                      requiredYieldsExtractor.ExtractFrom(implementation, errorCollector)
                  )
                : new ScenarioInfo
                 (
                      type,
                      type,
                      fullName,
                      friendlyName,
                      description,
                      explicitelyRequires
                  );
        }
    }
}
