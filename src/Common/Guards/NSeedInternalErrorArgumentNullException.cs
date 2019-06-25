using System;
using System.Runtime.Serialization;

namespace NSeed.Guards
{
    internal class NSeedInternalErrorArgumentNullException : ArgumentNullException
    {
        public NSeedInternalErrorArgumentNullException() { }

        public NSeedInternalErrorArgumentNullException(string paramName) : base(paramName) { }

        public NSeedInternalErrorArgumentNullException(string paramName, string message) : base(paramName, message) { }

        protected NSeedInternalErrorArgumentNullException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }
    }
}