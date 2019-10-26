using NSeed.MetaInfo;

namespace NSeed.Discovery.ErrorMessages
{
    internal static partial class Errors
    {
        internal static class FriendlyName
        {
            internal static readonly string MustNotBeNullErrorCode = ErrorCodePrefixes.FriendlyName.Prefix + "01";
            internal static readonly string MustNotBeEmptyStringErrorCode = ErrorCodePrefixes.FriendlyName.Prefix + "02";
            internal static readonly string MustNotBeWhitespaceErrorCode = ErrorCodePrefixes.FriendlyName.Prefix + "03";

            internal static Error MustNotBeNull { get; } = new Error
            (
                MustNotBeNullErrorCode,
                $"The value of the {nameof(FriendlyNameAttribute)} must not be null."
            );

            internal static Error MustNotBeEmptyString { get; } = new Error
            (
                MustNotBeEmptyStringErrorCode,
                $"The value of the {nameof(FriendlyNameAttribute)} must not be an empty string."
            );

            internal static Error MustNotBeWhitespace { get; } = new Error
            (
                MustNotBeWhitespaceErrorCode,
                $"The value of the {nameof(FriendlyNameAttribute)} must not be whitespace."
            );
        }
    }
}
