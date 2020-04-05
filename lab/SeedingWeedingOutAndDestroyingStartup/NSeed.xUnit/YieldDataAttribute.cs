using NSeed.Discovery.SeedBucket.ReflectionBased;
using NSeed.Extensions;
using NSeed.Sdk;
using NSeed.Seeding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit.Sdk;

namespace NSeed.Xunit
{
    public class YieldDataAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var parameters = testMethod.GetParameters();

            // TODO: Proper detailed error message.
            if (!parameters.All(parameter => parameter.ParameterType.IsYieldType())) throw new Exception("Some of the parameters are not yield types.");

            var seedingStartupType = FindSeedingStartupType();

            var outputSink = new InternalQueueOutputSink();
            var seedBucketInfoBuilder = new ReflectionBasedSeedBucketInfoBuilder();

            var seeder = new Seeder(seedBucketInfoBuilder, outputSink);

            yield return seeder.GetYieldsFor(seedingStartupType, parameters.Select(parameter => parameter.ParameterType).ToArray()).Result;

            Type? FindSeedingStartupType()
            {
                var seedingStartupAttribute = testMethod.GetCustomAttribute<UseSeedingStartupAttribute>();
                if (seedingStartupAttribute != null) return seedingStartupAttribute.SeedingStartupType; // TODO: Check that it is not null, that is seeding startup type etc.

                seedingStartupAttribute = testMethod.DeclaringType.GetCustomAttribute<UseSeedingStartupAttribute>();
                if (seedingStartupAttribute != null) return seedingStartupAttribute.SeedingStartupType; // TODO: Check that it is not null, that is seeding startup type etc.

                seedingStartupAttribute = testMethod.DeclaringType.Assembly.GetCustomAttribute<UseSeedingStartupAttribute>();
                if (seedingStartupAttribute != null) return seedingStartupAttribute.SeedingStartupType; // TODO: Check that it is not null, that is seeding startup type etc.

                return null;
            }
        }
    }
}
