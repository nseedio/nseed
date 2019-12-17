using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using NSeed.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit.Abstractions;

namespace NSeed.Tests.Unit
{
    internal class SeedAssemblyBuilder
    {
        private abstract class BaseDescription
        {
            public string? Name { get; set; }
            public string? FriendlyName { get; set; }
            public string? Description { get; set; }
        }

        private class SeedBucketDescription : BaseDescription { }

        private abstract class SeedableDescription : BaseDescription
        {
            public IEnumerable<string> Requires { get; set; } = Array.Empty<string>();
        }

        private class SeedDescription : SeedableDescription
        {
            public IEnumerable<string> Entities { get; set; } = Array.Empty<string>();
            public bool HasYield { get; set; }
            public bool Fails { get; set; }
            public bool HasAlreadyYielded { get; set; }
        }

        private class ScenarioDescription : SeedableDescription { }

        private static readonly List<MetadataReference> CommonMetadataReferences = new List<MetadataReference>();
        private readonly ITestOutputHelper output;

        static SeedAssemblyBuilder()
        {
            CommonMetadataReferences.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            CommonMetadataReferences.Add(MetadataReference.CreateFromFile(typeof(SeedBucket).Assembly.Location));
            foreach (var assembly in GetAdditionalAssemblies())
            {
                CommonMetadataReferences.Add(MetadataReference.CreateFromFile(assembly.Location));
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
                    if (result.Count >= neededReferencedAssemblyNames.Length) return;

                    var referencedAssemblyNamesAndAssemblies = parentAssembly
                        .GetReferencedAssemblies()
                        .Where(assemblyName => neededReferencedAssemblyNames.Contains(assemblyName.Name) && !result.ContainsKey(assemblyName))
                        .Select(assemblyName => (assemblyName, assembly: Assembly.Load(assemblyName)));

                    foreach (var (assemblyName, assembly) in referencedAssemblyNamesAndAssemblies)
                    {
                        result.Add(assemblyName, assembly);

                        FindAdditionalAssembliesRecursively(assembly);
                    }
                }
            }
        }

        public SeedAssemblyBuilder(ITestOutputHelper output)
        {
            this.output = output;
        }

        private readonly Dictionary<string, SeedBucketDescription> seedBuckets = new Dictionary<string, SeedBucketDescription>();
        private readonly Dictionary<string, SeedDescription> seeds = new Dictionary<string, SeedDescription>();
        private readonly Dictionary<string, ScenarioDescription> scenarios = new Dictionary<string, ScenarioDescription>();
        private readonly List<MetadataReference> metadataReferences = new List<MetadataReference>();

        private const string DefaultTestSeedBucketTypeName = "DefaultTestSeedBucket";
        public SeedAssemblyBuilder AddSeedBucket(string? seedBucketTypeName = null, string? seedBucketFriendlyName = null, string? seedBucketDescription = null)
        {
            seedBucketTypeName ??= DefaultTestSeedBucketTypeName;

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

        private const string DefaultTestSeedTypeName = "DefaultTestSeed";
        public SeedAssemblyBuilder AddSeed(
            string? seedTypeName = null,
            string? seedFriendlyName = null,
            string? seedDescription = null,
            bool hasYield = false,
            bool fails = false,
            bool hasAlreadyYielded = false,
            // TODO: Bug in StyleCop.
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
            string[]? entities = null,
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
            params string[] requires)
        {
            seedTypeName ??= DefaultTestSeedTypeName;
            entities ??= Array.Empty<string>();
            requires ??= Array.Empty<string>();

            seedTypeName.Where(char.IsWhiteSpace).Should().BeEmpty("seed type name must not contain white spaces");
            seeds.ContainsKey(seedTypeName).Should().BeFalse($"seed type can be added only once. Seed with the name '{seedTypeName}' has already been added to the seed assembly");
            requires.Should().NotContainNulls();

            seeds.Add(seedTypeName, new SeedDescription
            {
                Name = seedTypeName,
                FriendlyName = seedFriendlyName,
                Description = seedDescription,
                HasYield = hasYield,
                Fails = fails,
                HasAlreadyYielded = hasAlreadyYielded,
                Entities = entities,
                Requires = requires
            });

            return this;
        }

        private const string DefaultTestScenarioTypeName = "DefaultTestScenario";
        public SeedAssemblyBuilder AddScenario(string? scenarioTypeName = null, string? scenarioFriendlyName = null, string? scenarioDescription = null, params string[] requires)
        {
            scenarioTypeName ??= DefaultTestScenarioTypeName;
            requires ??= Array.Empty<string>();

            scenarioTypeName.Where(char.IsWhiteSpace).Should().BeEmpty("scenario type name must not contain white spaces");
            seeds.ContainsKey(scenarioTypeName).Should().BeFalse($"scenario type can be added only once. Scenario with the name '{scenarioTypeName}' has already been added to the seed assembly");
            requires.Should().NotContainNulls();

            scenarios.Add(scenarioTypeName, new ScenarioDescription
            {
                Name = scenarioTypeName,
                FriendlyName = scenarioFriendlyName,
                Description = scenarioDescription,
                Requires = requires
            });

            return this;
        }

        public SeedAssemblyBuilder AddReference(MetadataReference metadataReference)
        {
            return AddReferences(metadataReference);
        }

        public SeedAssemblyBuilder AddReferences(params MetadataReference[] metadataReferences)
        {
            metadataReferences.Should().NotBeNull();
            metadataReferences.Should().NotContainNulls();

            foreach (var reference in metadataReferences) this.metadataReferences.Add(reference);

            return this;
        }

        public Assembly BuildAssembly()
        {
            using MemoryStream ms = BuildAssemblyStream();
            return Assembly.Load(ms.ToArray());
        }

        public Assembly BuildPersistedAssembly()
        {
            using MemoryStream ms = BuildAssemblyStream();
            var assemblyFileName = Path.GetTempFileName();
            using (FileStream fs = File.OpenWrite(assemblyFileName))
            {
                ms.CopyTo(fs);
            }

            return Assembly.LoadFrom(assemblyFileName);
        }

        private MemoryStream BuildAssemblyStream()
        {
            var sourceCode = CreateSourceCode();
            output.WriteLine(sourceCode);

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            CSharpCompilation compilation = CSharpCompilation.Create(
                Path.GetRandomFileName(),
                syntaxTrees: new[] { syntaxTree },
                references: CommonMetadataReferences.Union(metadataReferences),
                options: new CSharpCompilationOptions(OutputKind.ConsoleApplication));

            var memoryStream = new MemoryStream();
            EmitResult emitResult = compilation.Emit(memoryStream);

            emitResult.Success.Should().BeTrue(
                $"compilation must not have errors. Compilation had the following errors:{Environment.NewLine}" +
                $"{emitResult.Diagnostics.Select(diagnostics => diagnostics.GetMessage()).Aggregate(string.Empty, (result, current) => $"{current}{Environment.NewLine}{result}")}" +
                $"The generated source code was:{Environment.NewLine}{Environment.NewLine}" +
                sourceCode);

            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;

            string CreateSourceCode()
            {
                var seedBucketsSourceCode = seedBuckets
                    .Values
                    .Select(CreateSeedBucketSoruceCode)
                    .Aggregate(string.Empty, (result, current) => $"{current}{Environment.NewLine}{result}");

                var seedsSourceCode = seeds
                    .Values
                    .Select(CreateSeedSourceCode)
                    .Aggregate(string.Empty, (result, current) => $"{current}{Environment.NewLine}{result}");

                var scenariosSourceCode = scenarios
                    .Values
                    .Select(CreateScenarioSourceCode)
                    .Aggregate(string.Empty, (result, current) => $"{current}{Environment.NewLine}{result}");

                return
                    $"using System;{Environment.NewLine}" +
                    $"using System.Threading.Tasks;{Environment.NewLine}" +
                    $"using NSeed;{Environment.NewLine}" +
                    Environment.NewLine +
                    $"public class Program {{ public static void Main() {{ }} }}{Environment.NewLine}" +
                    Environment.NewLine +
                    seedBucketsSourceCode +
                    Environment.NewLine +
                    seedsSourceCode +
                    Environment.NewLine +
                    scenariosSourceCode +
                    Environment.NewLine;

                static string CreateSeedBucketSoruceCode(SeedBucketDescription seedBucketDescription)
                {
                    var (friendlyNameAttribute, descriptionAttribute) = CreateFriendlyNameAndDescriptionAttributes(seedBucketDescription);

                    return $"{friendlyNameAttribute}{descriptionAttribute}public class {seedBucketDescription.Name} : SeedBucket {{ }}";
                }

                static string CreateSeedSourceCode(SeedDescription seedDescription)
                {
                    var (friendlyNameAttribute, descriptionAttribute) = CreateFriendlyNameAndDescriptionAttributes(seedDescription);

                    var yield = seedDescription.HasYield
                        ? $"    public class Yield : YieldOf<{seedDescription.Name}> {{ }}{Environment.NewLine}"
                        : string.Empty;

                    var requiresAttributes = CreateRequiresAttributes(seedDescription);

                    var iseedInterface = CreateISeedInterface();

                    var seedMethodBody = CreateSeedMethodBody();

                    var hasAlreadyYieldedMethodBody = CreateHasAlreadyYieldedMethodBody();

                    return
                        $"{requiresAttributes}" +
                        $"{friendlyNameAttribute}" +
                        $"{descriptionAttribute}" +
                        $"public class {seedDescription.Name} : {iseedInterface}{Environment.NewLine}" +
                        $"{{{Environment.NewLine}" +
                        $"    public Task Seed() => {seedMethodBody};{Environment.NewLine}" +
                        $"    public Task<bool> HasAlreadyYielded() => {hasAlreadyYieldedMethodBody};{Environment.NewLine}" +
                        yield +
                        $"}}{Environment.NewLine}";

                    string CreateHasAlreadyYieldedMethodBody()
                        => $"Task.FromResult({seedDescription.HasAlreadyYielded.ToString().ToLowerInvariant()})";

                    string CreateSeedMethodBody()
                    {
                        return seedDescription.Fails
                            ? @"throw new Exception($""The seed {GetType().Name} has failed."")"
                            : "Task.CompletedTask";
                    }

                    string CreateISeedInterface()
                    {
                        var entites = seedDescription.Entities.Any()
                            ? $"<{string.Join(", ", seedDescription.Entities)}>"
                            : string.Empty;

                        return $"ISeed{entites}";
                    }
                }

                static string CreateScenarioSourceCode(ScenarioDescription scenarioDescription)
                {
                    var (friendlyNameAttribute, descriptionAttribute) = CreateFriendlyNameAndDescriptionAttributes(scenarioDescription);

                    var requiresAttributes = CreateRequiresAttributes(scenarioDescription);

                    return
                        $"{requiresAttributes}" +
                        $"{friendlyNameAttribute}" +
                        $"{descriptionAttribute}" +
                        $"public class {scenarioDescription.Name} : IScenario {{}}{Environment.NewLine}";
                }

                static string CreateRequiresAttributes(SeedableDescription seedableDescription)
                {
                    return seedableDescription
                        .Requires
                        .Select(requiredSeedableName => $"[Requires(typeof({requiredSeedableName}))]")
                        .Aggregate(string.Empty, (result, current) => $"{current}{Environment.NewLine}{result}");
                }

                static (string friendlyNameAttribute, string descriptionAttribute) CreateFriendlyNameAndDescriptionAttributes(BaseDescription description)
                {
                    var friendlyNameAttribute = description.FriendlyName == null
                        ? string.Empty
                        : $@"[FriendlyName(""{description.FriendlyName}"")]{Environment.NewLine}";
                    var descriptionAttribute = description.Description == null
                        ? string.Empty
                        : $@"[Description(""{description.Description}"")]{Environment.NewLine}";

                    return (friendlyNameAttribute, descriptionAttribute);
                }
            }
        }
    }
}
