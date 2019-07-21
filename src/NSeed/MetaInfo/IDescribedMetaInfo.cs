namespace NSeed.MetaInfo
{
    /// <summary>
    /// A <see cref="MetaInfo"/> that provides a <see cref="FriendlyName"/>
    /// and a <see cref="Description"/>.
    /// </summary>
    public interface IDescribedMetaInfo
    {
        /// <summary>
        /// Gets the friendly name of the NSeed abstraction.
        /// </summary>
        /// <returns>
        /// If the NSeed abstraction's implementation is annotated with the <see cref="FriendlyNameAttribute"/>
        /// the <see cref="FriendlyNameAttribute.FriendlyName"/> is returned.
        /// <br/>
        /// Otherwise, the humanized version of the implementation type name is returned.
        /// </returns>
        string FriendlyName { get; }

        /// <summary>
        /// Gets the description of the NSeed abstraction.
        /// </summary>
        /// <returns>
        /// If the NSeed abstraction's implementation is annotated with the <see cref="DescriptionAttribute"/>
        /// the <see cref="DescriptionAttribute.Description"/> is returned.
        /// <br/>
        /// Otherwise, the <see cref="string.Empty"/> is returned.
        /// </returns>
        string Description { get; }
    }
}
