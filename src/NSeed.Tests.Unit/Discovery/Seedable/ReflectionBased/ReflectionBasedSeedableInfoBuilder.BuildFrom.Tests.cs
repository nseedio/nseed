using System;
using Xunit;
using FluentAssertions;
using NSeed.Discovery;
using NSeed.MetaInfo;
using NSeed.Extensions;
using NSeed.Discovery.Seedable.ReflectionBased;
using System.Linq;

namespace NSeed.Tests.Unit.Discovery.Seedable.ReflectionBased
{
    public class ReflectionBasedSeedableInfoBuilderﾠBuildFrom
    {
        private readonly IMetaInfoBuilder<Type, SeedableInfo> builder = new ReflectionBasedSeedableInfoBuilder();

        [Fact]
        public void ReturnsﾠexpectedﾠfullyﾠpopulatedﾠSeedInfo()
        {
            Type type = typeof(FullyPopulatedSeed);

            var expected = new SeedInfo
            (
                type,
                type.FullName,
                SomeFriendlyName,
                SomeDescription,
                new SeedableInfo[]
                {
                    CreateSeedInfoForMinimalSeedType(typeof(MinimalSeed)),
                    CreateSeedInfoForMinimalSeedType(typeof(AdditionalMinimalSeed)),
                    CreateSeedInfoForMinimalScenarioType(typeof(AdditionalMinimalScenario)),
                    new ScenarioInfo
                    (
                        typeof(FullyPopulatedScenario),
                        typeof(FullyPopulatedScenario).FullName,
                        SomeFriendlyName,
                        SomeDescription,
                        new SeedableInfo[]
                        {
                            CreateSeedInfoForMinimalSeedType(typeof(MinimalSeed)),
                            CreateSeedInfoForMinimalSeedType(typeof(AdditionalMinimalSeed)),
                            CreateSeedInfoForMinimalScenarioType(typeof(AdditionalMinimalScenario))
                        }
                    )
                },
                new[]
                {
                    new EntityInfo(typeof(object), typeof(object).FullName),
                    new EntityInfo(typeof(string), typeof(string).FullName),
                    new EntityInfo(typeof(int), typeof(int).FullName)
                },
                new ProvidedYieldInfo(typeof(FullyPopulatedSeed.Yield), typeof(FullyPopulatedSeed.Yield).FullName)
            );

            builder.BuildFrom(type)
                .Should()
                .BeEquivalentTo(expected, option =>
                    option.IgnoringCyclicReferences().WithoutStrictOrdering());
        }

        private const string SomeFriendlyName = "Some friendly name";
        private const string SomeDescription = "Some description";
        [Requires(typeof(AdditionalMinimalSeed))]
        [Requires(typeof(MinimalSeed))]
        [Requires(typeof(FullyPopulatedScenario))]
        [Requires(typeof(AdditionalMinimalScenario))]
        [FriendlyName(SomeFriendlyName)]
        [Description(SomeDescription)]
        private class FullyPopulatedSeed : BaseTestSeed, ISeed<object, string, int>
        {
            public class Yield : YieldOf<FullyPopulatedSeed> { }
        }
        private class AdditionalMinimalSeed : BaseTestSeed { }

        [Fact]
        public void ReturnsﾠexpectedﾠfullyﾠpopulatedﾠScenarioInfo()
        {
            Type type = typeof(FullyPopulatedScenario);

            var expected = new ScenarioInfo
            (
                type,
                type.FullName,
                SomeFriendlyName,
                SomeDescription,
                new SeedableInfo[]
                {
                    CreateSeedInfoForMinimalSeedType(typeof(MinimalSeed)),
                    CreateSeedInfoForMinimalSeedType(typeof(AdditionalMinimalSeed)),
                    CreateSeedInfoForMinimalScenarioType(typeof(AdditionalMinimalScenario))
                }
            );

            builder.BuildFrom(type).Should().BeEquivalentTo(expected, options => options.WithoutStrictOrdering());
        }

        [Requires(typeof(AdditionalMinimalSeed))]
        [Requires(typeof(MinimalSeed))]
        [Requires(typeof(AdditionalMinimalScenario))]
        [FriendlyName(SomeFriendlyName)]
        [Description(SomeDescription)]
        private class FullyPopulatedScenario : BaseTestScenario { }
        private class AdditionalMinimalScenario : BaseTestScenario { }

        [Fact]
        public void ReturnsﾠexpectedﾠminimalﾠSeedInfo()
        {
            Type type = typeof(MinimalSeed);

            var expected = CreateSeedInfoForMinimalSeedType(type);

            builder.BuildFrom(type).Should().BeEquivalentTo(expected);
        }
        private class MinimalSeed : BaseTestSeed { }

        [Fact]
        public void ReturnsﾠexpectedﾠminimalﾠScenarioInfo()
        {
            Type type = typeof(MinimalScenario);

            var expected = CreateSeedInfoForMinimalScenarioType(type);

            builder.BuildFrom(type).Should().BeEquivalentTo(expected);
        }
        private class MinimalScenario : BaseTestScenario { }

        [Fact]
        public void ReturnsﾠexactlyﾠtheﾠsameﾠSeedInfoﾠforﾠtheﾠsameﾠseedﾠtype()
        {
            Type type01 = typeof(MinimalSeed);
            Type type02 = typeof(MinimalSeed);

            builder.BuildFrom(type01).Should().BeSameAs(builder.BuildFrom(type02));
        }

        [Fact]
        public void ReturnsﾠexactlyﾠtheﾠsameﾠScenarioInfoﾠforﾠtheﾠsameﾠscenarioﾠtype()
        {
            Type type01 = typeof(MinimalScenario);
            Type type02 = typeof(MinimalScenario);

            builder.BuildFrom(type01).Should().BeSameAs(builder.BuildFrom(type02));
        }

        [Fact]
        public void ReturnsﾠexactlyﾠtheﾠsameﾠSeedInfoﾠforﾠtheﾠseedﾠtypeﾠthatﾠoccursﾠseveralﾠtimesﾠinﾠtheﾠseedableﾠgraph()
        {
            var seedThatRequiresOtherSeed = (SeedInfo)builder.BuildFrom(typeof(SeedThatRequiresOtherSeedSeveralTimes));
            var otherSeed = (SeedInfo)builder.BuildFrom(typeof(OtherSeed));

            var explicitlyRequired = seedThatRequiresOtherSeed.ExplicitlyRequiredSeedables.First(seedableInfo => seedableInfo.FullName == otherSeed.FullName);
            var requiredThroughScenario = seedThatRequiresOtherSeed
                .ExplicitlyRequiredSeedables
                .First(seedableInfo => seedableInfo.FullName == typeof(ScenarioThatRequiresOtherSeed).FullName)
                .ExplicitlyRequiredSeedables.First(seedableInfo => seedableInfo.FullName == otherSeed.FullName);

            // TODO-IG: Add that it depends on its own yield (implicit dependency).

            explicitlyRequired.Should().BeSameAs(otherSeed);
            requiredThroughScenario.Should().BeSameAs(otherSeed);
        }
        [Requires(typeof(ScenarioThatRequiresOtherSeed))]
        [Requires(typeof(OtherSeed))]
        private class SeedThatRequiresOtherSeedSeveralTimes : BaseTestSeed { } // TODO-IG: Add that it depends on its own yield (implicit dependency).
        private class OtherSeed : BaseTestSeed { }
        [Requires(typeof(OtherSeed))]
        private class ScenarioThatRequiresOtherSeed : BaseTestScenario { }

        [Fact]
        public void ReturnsﾠexactlyﾠtheﾠsameﾠScenarioInfoﾠforﾠtheﾠscenarioﾠtypeﾠthatﾠoccursﾠseveralﾠtimesﾠinﾠtheﾠseedableﾠgraph()
        {
            var scenarioThatRequiresOtherScenario = (ScenarioInfo)builder.BuildFrom(typeof(ScenarioThatRequiresOtherScenarioSeveralTimes));
            var otherScenario = (ScenarioInfo)builder.BuildFrom(typeof(OtherScenario));

            var explicitlyRequired = scenarioThatRequiresOtherScenario.ExplicitlyRequiredSeedables.First(seedableInfo => seedableInfo.FullName == otherScenario.FullName);
            var requiredThroughScenario = scenarioThatRequiresOtherScenario
                .ExplicitlyRequiredSeedables
                .First(seedableInfo => seedableInfo.FullName == typeof(ScenarioThatRequiresOtherScenario).FullName)
                .ExplicitlyRequiredSeedables.First(seedableInfo => seedableInfo.FullName == otherScenario.FullName);

            // TODO-IG: Add that it depends on its own yield (implicit dependency).

            explicitlyRequired.Should().BeSameAs(otherScenario);
            requiredThroughScenario.Should().BeSameAs(otherScenario);
        }
        [Requires(typeof(ScenarioThatRequiresOtherScenario))]
        [Requires(typeof(OtherScenario))]
        private class ScenarioThatRequiresOtherScenarioSeveralTimes : BaseTestScenario { }
        private class OtherScenario : BaseTestScenario { }
        [Requires(typeof(OtherScenario))]
        private class ScenarioThatRequiresOtherScenario : BaseTestScenario { }

        [Fact]
        public void Ignoresﾠdirectﾠcircularﾠdependencyﾠofﾠseeds()
        {
            Type type = typeof(SeedThatRequiresItself);

            var seedInfo = (SeedInfo)builder.BuildFrom(type);

            seedInfo.ExplicitlyRequiredSeedables.Should().BeEmpty();
        }
        [Requires(typeof(SeedThatRequiresItself))]
        private class SeedThatRequiresItself : BaseTestSeed { }

        // TODO-IG: Ignoresﾠdirectﾠcircularﾠdependencyﾠofﾠseedsﾠviaﾠseedsﾠownﾠyield()

        [Fact]
        public void Ignoresﾠdirectﾠcircularﾠdependencyﾠofﾠscenarios()
        {
            Type type = typeof(ScenarioThatRequiresItself);

            var scenarioInfo = (ScenarioInfo)builder.BuildFrom(type);

            scenarioInfo.ExplicitlyRequiredSeedables.Should().BeEmpty();
        }
        [Requires(typeof(ScenarioThatRequiresItself))]
        private class ScenarioThatRequiresItself : BaseTestScenario { }

        [Fact]
        public void Ignoresﾠindirectﾠcircularﾠdependencyﾠofﾠseedsﾠoverﾠseeds()
        {
            Type type = typeof(SeedThatIndirectlyRequiresItself);

            var seedInfo = (SeedInfo)builder.BuildFrom(type);

            seedInfo.ExplicitlyRequiredSeedables.Should().Contain(builder.BuildFrom(typeof(SeedA)));
            seedInfo.ExplicitlyRequiredSeedables.Should().NotContain(seedInfo);
        }
        [Requires(typeof(SeedA))]
        private class SeedThatIndirectlyRequiresItself : BaseTestSeed { }
        [Requires(typeof(SeedThatIndirectlyRequiresItself))]
        private class SeedA : BaseTestSeed { }

        [Fact]
        public void Ignoresﾠindirectﾠcircularﾠdependencyﾠofﾠscenariosﾠoverﾠscenarios()
        {
            Type type = typeof(ScenarioThatIndirectlyRequiresItself);

            var scenarioInfo = (ScenarioInfo)builder.BuildFrom(type);

            scenarioInfo.ExplicitlyRequiredSeedables.Should().Contain(builder.BuildFrom(typeof(ScenarioA)));
            scenarioInfo.ExplicitlyRequiredSeedables.Should().NotContain(scenarioInfo);
        }
        [Requires(typeof(ScenarioA))]
        private class ScenarioThatIndirectlyRequiresItself : BaseTestScenario { }
        [Requires(typeof(ScenarioThatIndirectlyRequiresItself))]
        private class ScenarioA : BaseTestScenario { }

        [Fact]
        public void Ignoresﾠindirectﾠcircularﾠdependencyﾠofﾠseedsﾠandﾠscenariosﾠoverﾠotherﾠseedsﾠandﾠscenarios()
        {
            var seedZero = (SeedInfo)builder.BuildFrom(typeof(SeedZero));

            var seedOne = seedZero.ExplicitlyRequiredSeedables.First(seedable => seedable.FullName == typeof(SeedOne).FullName);
            var scenarioTwo = seedOne.ExplicitlyRequiredSeedables.First(seedable => seedable.FullName == typeof(ScenarioTwo).FullName);
            var seedThree = scenarioTwo.ExplicitlyRequiredSeedables.First(seedable => seedable.FullName == typeof(SeedThree).FullName);
            var scenarioFour = seedThree.ExplicitlyRequiredSeedables.First(seedable => seedable.FullName == typeof(ScenarioFour).FullName);
            var seedFive = scenarioFour.ExplicitlyRequiredSeedables.First(seedable => seedable.FullName == typeof(SeedFive).FullName);
            var scenarioSix = seedFive.ExplicitlyRequiredSeedables.First(seedable => seedable.FullName == typeof(ScenarioSix).FullName);
            var seedSeven = scenarioSix.ExplicitlyRequiredSeedables.First(seedable => seedable.FullName == typeof(SeedSeven).FullName);
            var scenarioEight = seedSeven.ExplicitlyRequiredSeedables.First(seedable => seedable.FullName == typeof(ScenarioEight).FullName);

            seedZero.ExplicitlyRequiredSeedables.Should().Contain(seedOne);

            seedOne.ExplicitlyRequiredSeedables.Should().Contain(scenarioTwo);

            scenarioTwo.ExplicitlyRequiredSeedables.Should().Contain(seedThree);

            seedThree.ExplicitlyRequiredSeedables.Should().Contain(scenarioFour);
            seedThree.ExplicitlyRequiredSeedables.Should().NotContain(seedOne);

            scenarioFour.ExplicitlyRequiredSeedables.Should().Contain(seedFive);

            seedFive.ExplicitlyRequiredSeedables.Should().Contain(scenarioSix);

            scenarioSix.ExplicitlyRequiredSeedables.Should().Contain(seedSeven);
            scenarioSix.ExplicitlyRequiredSeedables.Should().NotContain(seedThree);
            scenarioSix.ExplicitlyRequiredSeedables.Should().NotContain(scenarioFour);

            seedSeven.ExplicitlyRequiredSeedables.Should().Contain(scenarioEight);

            scenarioEight.ExplicitlyRequiredSeedables.Should().BeEmpty();
        }
        // SeedZero -> SeedOne~ -> ScenarioTwo -> SeedThree~ -> ScenarioFour~ -> SeedFive -> ScenarioSix -> SeedThree*
        //                                                   -> SeedOne*                                 -> SeedSeven -> ScenarioEight~ -> ScenarioEight*
        //                                                                                               -> ScenarioFour*
        [Requires(typeof(SeedOne))]
        private class SeedZero : BaseTestSeed { }
        [Requires(typeof(ScenarioTwo))]
        private class SeedOne : BaseTestSeed { }
        [Requires(typeof(SeedThree))]
        private class ScenarioTwo : BaseTestScenario { }
        [Requires(typeof(ScenarioFour))]
        [Requires(typeof(SeedOne))]
        private class SeedThree : BaseTestSeed { }
        [Requires(typeof(SeedFive))]
        private class ScenarioFour : BaseTestScenario { }
        [Requires(typeof(ScenarioSix))]
        private class SeedFive : BaseTestSeed { }
        [Requires(typeof(SeedThree))]
        [Requires(typeof(SeedSeven))]
        private class ScenarioSix : BaseTestScenario { }
        [Requires(typeof(ScenarioEight))]
        private class SeedSeven : BaseTestSeed { }
        [Requires(typeof(ScenarioEight))]
        private class ScenarioEight : BaseTestScenario { }


        private static SeedInfo CreateSeedInfoForMinimalSeedType(Type minimalSeedType)
        {
            return new SeedInfo
            (
                minimalSeedType,
                minimalSeedType.FullName,
                minimalSeedType.Name.Humanize(),
                string.Empty,
                Array.Empty<SeedableInfo>(),
                Array.Empty<EntityInfo>(),
                null
            );
        }

        private static ScenarioInfo CreateSeedInfoForMinimalScenarioType(Type minimalScenarioType)
        {
            return new ScenarioInfo
            (
                minimalScenarioType,
                minimalScenarioType.FullName,
                minimalScenarioType.Name.Humanize(),
                string.Empty,
                Array.Empty<SeedableInfo>()
            );
        }
    }
}