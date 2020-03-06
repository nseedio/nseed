using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Gets the errors that occur directly in the definition of the NSeed abstraction
        /// described with this meta info.
        /// <br/>
        /// To get the errors that occur in the NSeed abstraction and all its child abstractions
        /// use the <see cref="AllErrors"/> properties.
        /// </summary>
        public IReadOnlyCollection<Error> DirectErrors { get; }

        /// <summary>
        /// Gets the errors that occur in the definition of the NSeed abstraction
        /// described with this meta info and all its child abstractions, recursively.
        /// <br/>
        /// To get the errors that occur only in the NSeed abstraction itself without
        /// the errors in its child abstractions use the <see cref="DirectErrors"/> property.
        /// </summary>
        public IEnumerable<Error> AllErrors => GetAllErrors();

        /// <summary>
        /// Gets a value indicating whether there are errors that occur in the definition of the NSeed abstraction
        /// described with this meta info or any of its child abstractions, recursively.
        /// </summary>
        public bool HasAnyErrors => GetHasAnyErrors();

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaInfo"/> class.
        /// </summary>
        internal MetaInfo(object implementation, Type? type, string fullName, IReadOnlyCollection<Error> directErrors)
        {
            System.Diagnostics.Debug.Assert(directErrors.All(directError => directError != null));

            Implementation = implementation;
            Type = type;
            FullName = fullName;
            DirectErrors = directErrors;
        }

        /// <summary>
        /// Gets all direct child <see cref="MetaInfo"/>s of this meta info.
        /// </summary>
        /// <returns>All direct child <see cref="MetaInfo"/>s contained within this meta info.</returns>
        protected abstract IEnumerable<MetaInfo> GetDirectChildMetaInfos();

        private IEnumerable<Error> GetAllErrors()
        {
            foreach (var directError in DirectErrors)
                yield return directError;

            foreach (var directChildMetaInfo in GetDirectChildMetaInfos())
            {
                foreach (var error in directChildMetaInfo.AllErrors)
                    yield return error;
            }
        }

        private bool GetHasAnyErrors()
        {
            if (DirectErrors.Any()) return true;

            foreach (var directChildMetaInfo in GetDirectChildMetaInfos())
            {
                if (directChildMetaInfo.AllErrors.Any()) return true;
            }

            return false;
        }
    }
}
