using NSeed.MetaInfo;

namespace NSeed.Discovery.ErrorMessages
{
    internal static partial class Errors
    {
        internal static class Entity
        {
            internal static readonly string EntityMustNotBeNSeedTypeErrorCode = ErrorCodePrefixes.Entity.Prefix + "01";
            internal static readonly string EntityMustNotBeStaticTypeErrorCode = ErrorCodePrefixes.Entity.Prefix + "02";
            internal static readonly string EntityMustNotBeSeedErrorCode = ErrorCodePrefixes.Entity.Prefix + "03";
            internal static readonly string EntityMustNotBeScenarioErrorCode = ErrorCodePrefixes.Entity.Prefix + "04";

            internal static Error EntityMustNotBeNSeedType(string entityTypeName) => new Error
            (
                EntityMustNotBeNSeedTypeErrorCode,
                $"Entity must not be an NSeed type - {entityTypeName}."
            );

            internal static Error EntityMustNotBeStaticType(string entityTypeName) => new Error
            (
                EntityMustNotBeStaticTypeErrorCode,
                $"Entity must not be a static type - {entityTypeName}."
            );

            internal static Error EntityMustNotBeSeed(string entityTypeName) => new Error
            (
                EntityMustNotBeSeedErrorCode,
                $"Entity must not be a seed - {entityTypeName}."
            );

            internal static Error EntityMustNotBeScenario(string entityTypeName) => new Error
            (
                EntityMustNotBeScenarioErrorCode,
                $"Entity must not be a scenario - {entityTypeName}."
            );
        }
    }
}
