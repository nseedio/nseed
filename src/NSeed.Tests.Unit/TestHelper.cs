using FluentAssertions;
using Microsoft.CodeAnalysis;
using NSeed.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace NSeed.Tests.Unit
{
    internal static class TestHelper
    {
        public static Type GetGenericTypeParameter()
        {
            var type = typeof(ClassWithGenericMethod)
                .GetMethod("Method")
                .ReturnType;

            type.Should().Match(it => it.IsGenericParameter);

            return type;
        }

        private class ClassWithGenericMethod
        {
            public T Method<T>(T t) => t;
        }

        public static PropertyInfo GetPropertyWithName(this Type type, string propertyName)
        {
            return type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        }

        public static Type GetSeedBucketType(this Assembly assembly, string? seedBucketTypeName = null)
        {
            var seedBucketClasses = assembly.GetTypes()
                .Where(type => type.IsSeedBucketType() && (seedBucketTypeName == null || type.Name == seedBucketTypeName))
                .ToArray();

            seedBucketClasses
                .Should()
                .NotBeEmpty(seedBucketTypeName != null
                                ? $"seed bucket with the name {seedBucketTypeName} must exist in the assembly"
                                : $"at least one seed bucket must exist in the assembly");

            seedBucketClasses
                .Should()
                .HaveCount(1, seedBucketTypeName != null
                                ? $"exactly one seed bucket with the name {seedBucketTypeName} must exist in the assembly"
                                : $"exactly one seed bucket must exist in the assembly");

            return seedBucketClasses[0];
        }

        public static SeedBucket GetSeedBucket(this Assembly assembly, string? seedBucketTypeName = null)
        {
            return (SeedBucket)Activator.CreateInstance(assembly.GetSeedBucketType(seedBucketTypeName));
        }

        public static MetadataReference GetMetadataReference(this Assembly assembly)
        {
            assembly.Should().NotBeNull();
            assembly.Location
                .Should().NotBeNullOrWhiteSpace($"assembly must be persisted in order to get its metadata reference.{Environment.NewLine}" +
                                                $"If the assembly is build by {nameof(SeedAssemblyBuilder)} make sure to " +
                                                $"call the {nameof(SeedAssemblyBuilder.BuildPersistedAssembly)}() method.");

            return MetadataReference.CreateFromFile(assembly.Location);
        }
    }
}
