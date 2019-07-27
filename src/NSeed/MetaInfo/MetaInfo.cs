using System;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Base class for all NSeed meta info classes.
    /// Meta info classes describe concrete implementations of
    /// NSeed abstractions like for example <see cref="ISeed"/>
    /// or <see cref="SeedBucket"/>.
    /// </summary>
    public abstract class MetaInfo
    {
        /// <summary>
        /// Represents an unknown implementation of an NSeed abstraction.
        /// </summary>
        public static readonly object UnknownImplementation = new object();

        /// <summary>
        /// Gets the underlying implementation object of the NSeed abstraction
        /// described with this meta info.
        /// <br/>
        /// If the implementation object is <see cref="System.Type"/> it will be same as <see cref="Type"/>.
        /// <br/>
        /// If the implementation object is not know this property will be set to <see cref="UnknownImplementation"/> object.
        /// </summary>
        public object Implementation { get; }

        /// <summary>
        /// Gets the underlying implementation <see cref="System.Type"/> of the NSeed abstraction
        /// described with this meta info, if such <see cref="System.Type"/> exists; otherwise null.
        /// </summary>
        public Type? Type { get; }

        /// <summary>
        /// Gets the full name of the of the NSeed abstraction described with this meta info.
        /// </summary>
        /// <remarks>
        /// If the <see cref="Type"/> exists, this property is equal to its <see cref="Type.FullName"/>.
        /// </remarks>
        public string FullName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaInfo"/> class.
        /// </summary>
        internal MetaInfo(object implementation, Type? type, string fullName)
        {
            System.Diagnostics.Debug.Assert(implementation != null);
            System.Diagnostics.Debug.Assert(fullName != null);

            Implementation = implementation;
            Type = type;
            FullName = fullName;
        }
    }
}
