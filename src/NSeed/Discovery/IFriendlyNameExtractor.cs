using NSeed.MetaInfo;

namespace NSeed.Discovery
{
    internal static partial class Errors
    {
        // TODO-IG: Refactor this later on when we introduce IWeedOut. It should be a common thing.
        internal static class FriendlyName
        {
            internal static readonly Error MustNotBeNull = new Error
            (
                ErrorCodePrefixes.Seed.FriendlyName.Prefix + "01",
                $"The value of the seed {nameof(FriendlyNameAttribute)} must not be null."
            );

            internal static readonly Error MustNotBeEmptyString = new Error
            (
                ErrorCodePrefixes.Seed.FriendlyName.Prefix + "02",
                $"The value of the seed {nameof(FriendlyNameAttribute)} must not be empty string."
            );

            internal static readonly Error MustNotBeWhitespace = new Error
            (
                ErrorCodePrefixes.Seed.FriendlyName.Prefix + "03",
                $"The value of the seed {nameof(FriendlyNameAttribute)} must not be whitespace."
            );
        }
    }

    internal interface IFriendlyNameExtractor<TSource> : IExtractor<TSource, string>
        where TSource : class
    {
    }
}