﻿using NSeed.Guards;
using System;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// The base class of all NSeed meta info classes.
    /// Meta info classes fully describe concrete implementations of
    /// NSeed abstractions like e.g. <see cref="ISeed"/> or <see cref="SeedBucket"/>.
    /// </summary>
    public abstract class BaseMetaInfo
    {
        /// <summary>
        /// The underlying implementation <see cref="System.Type"/> of the NSeed abstraction
        /// described with this meta info, if such type exists; otherwise null.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// The full name of the of the NSeed abstraction described with this meta info.
        /// </summary>
        /// <remarks>
        /// If the <see cref="Type"/> exists, this property is equal to its <see cref="Type.FullName"/>.
        /// </remarks>
        public string FullName { get; }

        /// <summary>
        /// Creates new <see cref="BaseMetaInfo"/> with the specified type and full name.
        /// </summary>
        internal protected BaseMetaInfo(Type type, string fullName)
        {
            System.Diagnostics.Debug.Assert(!fullName.IsNullOrWhiteSpace());

            Type = type;
            FullName = fullName;
        }
    }
}