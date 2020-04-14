using NSeed.Guards;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// <see cref="IDescribedMetaInfo"/>'s extension methods.
    /// </summary>
    public static class DescribedMetaInfoExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the NSeed abstraction has <see cref="IDescribedMetaInfo.Description"/> property defined.<br/>
        /// The <see cref="IDescribedMetaInfo.Description"/> property is defined if its value is not empty or whitespace.
        /// </summary>
        /// <param name="describedMetaInfo">The <see cref="IDescribedMetaInfo"/>.</param>
        /// <returns>True if the description is defined.</returns>
        public static bool HasDescription(this IDescribedMetaInfo describedMetaInfo)
        {
            describedMetaInfo.MustNotBeNull();

            return !string.IsNullOrWhiteSpace(describedMetaInfo.Description);
        }
    }
}
