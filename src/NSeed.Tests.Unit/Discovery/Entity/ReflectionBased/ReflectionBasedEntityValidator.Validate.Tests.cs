using FluentAssertions;
using NSeed.Discovery;
using NSeed.Discovery.Entity.ReflectionBased;
using NSeed.Discovery.ErrorMessages;
using NSeed.Tests.Unit.Discovery.Seedable;
using System;
using Xunit;

namespace NSeed.Tests.Unit.Discovery.Entity.ReflectionBased
{
    public class ReflectionBasedEntityValidatorﾠValidate
    {
        private readonly IValidator<Type> validator = new ReflectionBasedEntityValidator();

        [Fact]
        public void ReportsﾠerrorﾠwhenﾠtypeﾠisﾠNSeedﾠtype()
        {
            Type type = typeof(ISeed);

            validator.Validate(type)
                .Should().ContainSingle()
                .And
                .ContainEquivalentOf(Errors.Entity.EntityMustNotBeNSeedType(type.FullName));
        }

        [Fact]
        public void Reportsﾠerrorﾠwhenﾠtypeﾠisﾠstaticﾠtype()
        {
            Type type = typeof(StaticClass);

            validator.Validate(type)
                .Should().ContainSingle()
                .And
                .ContainEquivalentOf(Errors.Entity.EntityMustNotBeStaticType(type.FullName));
        }
        private static class StaticClass { }

        [Fact]
        public void Reportsﾠerrorﾠwhenﾠtypeﾠisﾠseedﾠtype()
        {
            Type type = typeof(Seed);

            validator.Validate(type)
                .Should().ContainSingle()
                .And
                .ContainEquivalentOf(Errors.Entity.EntityMustNotBeSeed(type.FullName));
        }
        private class Seed : BaseTestSeed { }

        [Fact]
        public void Reportsﾠerrorﾠwhenﾠtypeﾠisﾠscenarioﾠtype()
        {
            Type type = typeof(Scenario);

            validator.Validate(type)
                .Should().ContainSingle()
                .And
                .ContainEquivalentOf(Errors.Entity.EntityMustNotBeScenario(type.FullName));
        }
        private class Scenario : BaseTestScenario { }

        [Fact]
        public void Doesﾠnotﾠreportﾠerrorsﾠwhenﾠtypeﾠhasﾠnoﾠerrors()
        {
            Type type = typeof(string);

            validator.Validate(type).Should().BeEmpty();
        }
    }
}
