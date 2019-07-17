﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace NSeed.Tests.Unit
{
    internal class SeedAssemblyBuilder
    {
        private class SeedBucketDescription
        {
            public string Name { get; set; }
            public string FriendlyName { get; set; }
            public string Description { get; set; }
        }

        private static readonly List<MetadataReference> metadataReferences = new List<MetadataReference>();

        static SeedAssemblyBuilder()
        {
            metadataReferences.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            metadataReferences.Add(MetadataReference.CreateFromFile(typeof(SeedBucket).Assembly.Location));
            foreach (var assembly in GetAdditionalAssemblies())
            {
                metadataReferences.Add(MetadataReference.CreateFromFile(assembly.Location));
            }

            IEnumerable<Assembly> GetAdditionalAssemblies()
            {
                // These are the additional assemblies needed by the NSeed assembly.
                string[] neededReferencedAssemblyNames = { "netstandard", "System.Runtime" };

                var result = new Dictionary<AssemblyName, Assembly>();

                FindAdditionalAssembliesRecursively(typeof(SeedBucket).Assembly);

                return result.Values;

                void FindAdditionalAssembliesRecursively(Assembly parentAssembly)
                {
                    var referencedAssemblyNamesAndAssemblies = parentAssembly
                        .GetReferencedAssemblies()
                        .Where(assemblyName => neededReferencedAssemblyNames.Contains(assemblyName.Name) && !result.ContainsKey(assemblyName))
                        .Select(assemblyName => (assemblyName, assembly: Assembly.Load(assemblyName)));

                    foreach (var assemblyNameAndAssembly in referencedAssemblyNamesAndAssemblies)
                    {
                        result.Add(assemblyNameAndAssembly.assemblyName, assemblyNameAndAssembly.assembly);

                        FindAdditionalAssembliesRecursively(assemblyNameAndAssembly.assembly);
                    }
                }
            }
        }

        private Dictionary<string, SeedBucketDescription> seedBuckets = new Dictionary<string, SeedBucketDescription>();

        private const string DefaultTestSeedBucketTypeName = "DefaultTestSeedBucket";
        public SeedAssemblyBuilder AddSeedBucket(string seedBucketTypeName = null, string seedBucketFriendlyName = null, string seedBucketDescription = null)
        {
            seedBucketTypeName = seedBucketTypeName ?? DefaultTestSeedBucketTypeName;

            seedBucketTypeName.Where(char.IsWhiteSpace).Should().BeEmpty("seed bucket type name must not contain white spaces");
            seedBuckets.ContainsKey(seedBucketTypeName).Should().BeFalse($"seed bucket type can be added only once. Seed bucket with the name '{seedBucketTypeName}' has already been added to the seed assembly");
            
            seedBuckets.Add(seedBucketTypeName, new SeedBucketDescription
            {
                Name = seedBucketTypeName,
                FriendlyName = seedBucketFriendlyName,
                Description = seedBucketDescription
            });

            return this;
        }

        public SeedBucket BuildAndGetSeedBucket(string seedBucketTypeName = null)
        {
            return (SeedBucket)Activator.CreateInstance(BuildAndGetSeedBucketType(seedBucketTypeName));
        }

        public Type BuildAndGetSeedBucketType(string seedBucketTypeName = null)
        {
            seedBucketTypeName = seedBucketTypeName ?? DefaultTestSeedBucketTypeName;

            var assembly = Build();
            var seedBucketClasses = assembly.GetTypes()
                .Where(type => typeof(SeedBucket).IsAssignableFrom(type) && type.Name == seedBucketTypeName)
                .ToArray();

            seedBucketClasses.Should().NotBeEmpty($"seed bucket with the name {seedBucketTypeName} must exist in the assembly");
            seedBucketClasses.Should().HaveCount(1, $"exactly one seed bucket with the name {seedBucketTypeName} must exist in the assembly");

            return seedBucketClasses[0];
        }

        public Assembly Build()
        {
            var sourceCode = CreateSourceCode();
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            CSharpCompilation compilation = CSharpCompilation.Create(
                Path.GetRandomFileName(),
                syntaxTrees: new [] { syntaxTree },
                references: metadataReferences,
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            using (var ms = new MemoryStream())
            {
                EmitResult emitResult = compilation.Emit(ms);

                emitResult.Success.Should().BeTrue(
                    $"compilation must not have errors. Compilation had the following errors:{Environment.NewLine}" +
                    $"{emitResult.Diagnostics.Select(diagnostics => diagnostics.GetMessage()).Aggregate(string.Empty, (result, current) => $"{current}{Environment.NewLine}{result}")}" +
                    $"The generated source code was:{Environment.NewLine}{Environment.NewLine}" +
                    sourceCode);

                ms.Seek(0, SeekOrigin.Begin);
                return Assembly.Load(ms.ToArray());
            }

            string CreateSourceCode()
            {
                var seedBucketsSourceCode = seedBuckets
                    .Values
                    .Select(CreateSeedBucketSoruceCode)
                    .Aggregate(string.Empty, (result, current) => $"{current}{Environment.NewLine}{result}");

                return 
                    $"using System;{Environment.NewLine}" +
                    $"using NSeed;{Environment.NewLine}" +
                    Environment.NewLine +
                    $"public class Program {{ public static void Main() {{ }} }}{Environment.NewLine}" +
                    seedBucketsSourceCode + 
                    Environment.NewLine;
                
                string CreateSeedBucketSoruceCode(SeedBucketDescription seedBucketDescription)
                {
                    var friendlyNameAttribute = seedBucketDescription.FriendlyName == null
                        ? string.Empty
                        : $@"[FriendlyName(""{seedBucketDescription.FriendlyName}"")]";
                    var descriptionAttribute = seedBucketDescription.Description == null
                        ? string.Empty
                        : $@"[Description(""{seedBucketDescription.Description}"")]";

                    return $"{friendlyNameAttribute}{descriptionAttribute}public class {seedBucketDescription.Name} : SeedBucket {{ }}";
                }
            }
        }
    }
}