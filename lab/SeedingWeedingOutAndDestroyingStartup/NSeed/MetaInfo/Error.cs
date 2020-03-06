using System.Linq;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Represents a single error in the definition of any of the NSeed abstractions.
    /// </summary>
    public sealed class Error
    {
        /// <summary>
        /// Gets the unique error code of this error.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; }

        internal Error(string code, string message)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(code));
            System.Diagnostics.Debug.Assert(code.All(char.IsDigit));
            System.Diagnostics.Debug.Assert(!string.IsNullOrWhiteSpace(message));

            Code = code;
            Message = message;
        }
    }
}
