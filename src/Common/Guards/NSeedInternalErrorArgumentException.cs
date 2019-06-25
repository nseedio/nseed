using System;
using System.Runtime.Serialization;

namespace NSeed.Guards
{
    internal class NSeedInternalErrorArgumentException : ArgumentException
    {
        public NSeedInternalErrorArgumentException() { }

        public NSeedInternalErrorArgumentException(string message) : base(message) { }

        public NSeedInternalErrorArgumentException(string message, string paramName) : base(message, paramName) { }

        protected NSeedInternalErrorArgumentException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
    }
}