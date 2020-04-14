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
        private readonly ISeedAlwaysRequiredExtractor<TSeedableImplementation> alwaysRequiredExtractor;
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
            ISeedAlwaysRequiredExtractor<TSeedableImplementation> alwaysRequiredExtractor,
            ISeedEntitiesExtractor<TSeedableImplementation> entitiesExtractor,
            ISeedProvidedYieldExtractor<TSeedableImplementation> providedYieldExtractor,
            Func<ISeedableInfoBuilder<TSeedableImplementation>, IExplicitlyRequiredSeedablesExtractor<TSeedableImplementation>> explicitlyRequiredSeedablesExtractorFactory,
            Func<ISeedableInfoBuilder<TSeedableImplementation>, ISeedRequiredYieldsExtractor<TSeedableImplementation>> requiredYieldsExtractorFactory,
            IMetaInfoPool<TSeedableImplementation, SeedableInfo> seedableInfoPool)
        {
            this.typeExtractor = typeExtractor;
            this.fullNameExtractor = fullNameExtractor;
            this.friendlyNameExtractor = friendlyNameExtractor;
            this.descriptionExtractor = descriptionExtractor;
            this.alwaysRequiredExtractor = alwaysRequiredExtractor;
            this.entitiesExtractor = entitiesExtractor;
            this.providedYieldExtractor = providedYieldExtractor;
            explicitlyRequiredSeedablesExtractor = explicitlyRequiredSeedablesExtractorFactory(this);
            requiredYieldsExtractor = requiredYieldsExtractorFactory(this);
            this.seedableInfoPool = seedableInfoPool;
        }

        SeedableInfo? IMetaInfoBuilder<TSeedableImplementation, SeedableInfo>.BuildFrom(TSeedableImplementation implementation)
        {
            if (buildChain.Contains(implementation)) return null;

            buildChain.Push(implementation);
            var result = seedableInfoPool.GetOrAdd(implementation, CreateSeedableInfo);
            buildChain.Pop();

            return result;
        }

        protected internal abstract bool IsSeedImplemenation(TSeedableImplementation implementation);

        private SeedableInfo CreateSeedableInfo(TSeedableImplementation implementation)
        {
            // A "Seedable" is actually a dicriminated union of ISeed and IScenario.
            // The way we distinguis between them everywhere is just a workaround
            // for non-existing discriminated unions in C#.

            bool isSeedImplementation = IsSeedImplemenation(implementation);

            Type? type = typeExtractor.ExtractFrom(implementation);
            string fullName = fullNameExtractor.ExtractFrom(implementation);
            string friendlyName = friendlyNameExtractor.ExtractFrom(implementation);
            string description = descriptionExtractor.ExtractFrom(implementation);
            bool isAlwaysRequired = alwaysRequiredExtractor.ExtractFrom(implementation);
            var explicitelyRequires = explicitlyRequiredSeedablesExtractor.ExtractFrom(implementation);

            return isSeedImplementation
                ? (SeedableInfo)new SeedInfo
                  (
                      implementation,
                      type,
                      fullName,
                      friendlyName,
                      description,
                      isAlwaysRequired,
                      explicitelyRequires,
                      entitiesExtractor.ExtractFrom(implementation),
                      providedYieldExtractor.ExtractFrom(implementation),
                      requiredYieldsExtractor.ExtractFrom(implementation),
                      Array.Empty<Error>()
                  )
                : new ScenarioInfo
                 (
                      implementation,
                      type,
                      fullName,
                      friendlyName,
                      description,
                      explicitelyRequires,
                      Array.Empty<Error>()
                  );
        }
    }
}
