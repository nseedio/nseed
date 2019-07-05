using NSeed.Guards;
using System.Linq;

namespace NSeed.MetaInfo
{
    /// <summary>
    /// Represents a single error in the definition of any of the NSeed abstractions.
    /// </summary>
    public sealed class Error
    {
        /// <summary>
        /// The unique error code of this error.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// The error message.
        /// </summary>
        public string Message { get; }

        internal Error(string code, string message)
        {
            System.Diagnostics.Debug.Assert(!code.IsNullOrWhiteSpace());
            System.Diagnostics.Debug.Assert(code.All(char.IsDigit));
            System.Diagnostics.Debug.Assert(!message.IsNullOrWhiteSpace());

            Code = code;
            Message = message;
        }
    }
}