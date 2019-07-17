using System;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Describes a single concrete <see cref="SeedBucket"/> implementation.
    /// </summary>
    public class SeedBucketInfo : MetaInfo, IDescribedMetaInfo
    {
        /// <inheritdoc/>
        public string FriendlyName { get; }

        /// <inheritdoc/>
        public string Description { get; }


        internal SeedBucketInfo(
            Type type,
            string fullName,
            string friendlyName,
            string description)
            :base(type, fullName)
        {            
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(friendlyName));
            System.Diagnostics.Debug.Assert(description != null);

            FriendlyName = friendlyName;
            Description = description;
        }
    }
}