using System;
using System.Runtime.Serialization;

namespace NSeed.Seeding
{
    /// <summary>
    /// Base class for all exceptions that can occur during the seeding.
    /// These exceptions are used internally within the core and the seeding.
    /// They are not meant to be used by the seeders.
    /// </summary>
    internal abstract class SeedingException : Exception
    {
        protected SeedingException() { }

        protected SeedingException(string message) : base(message) { }

        protected SeedingException(string message, Exception innerException) : base(message, innerException) { }

        protected SeedingException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
    }
}
