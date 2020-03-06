namespace NSeed.Abstractions
{
    /// <summary>
    /// Represent the standard output for textual messages,
    /// verbose messages, warnings, errors, and confirmations.
    /// </summary>
    public interface IOutputSink
    {
        /// <summary>
        /// Gets a value indicating whether the sink accepts verbose messages.
        /// <br/>
        /// Returns true if verbose messages are redirected to
        /// the output or false if they are ignored.
        /// </summary>
        bool AcceptsVerboseMessages { get; }

        /// <summary>
        /// Writes the current line terminator to the output sink.
        /// </summary>
        void WriteLine();

        /// <summary>
        /// Writes the specified <paramref name="message"/>,
        /// followed by the current line terminator,
        /// to the output sink.
        /// </summary>
        /// <param name="message">The message to write.</param>
        void WriteMessage(string message);

        /// <summary>
        /// Writes the specified <paramref name="verboseMessage"/>,
        /// followed by the current line terminator,
        /// to the output sink only if the verbose output
        /// is accepted.
        /// <br/>
        /// The verbose output is accepted if the value
        /// of the <see cref="AcceptsVerboseMessages"/> property is true.
        /// </summary>
        /// <param name="verboseMessage">The verbose message to write.</param>
        void WriteVerboseMessage(string verboseMessage);

        /// <summary>
        /// Writes the specified <paramref name="warning"/>,
        /// followed by the current line terminator,
        /// to the output sink.
        /// </summary>
        /// <param name="warning">The warning to write.</param>
        void WriteWarning(string warning);

        /// <summary>
        /// Writes the specified <paramref name="error"/>,
        /// followed by the current line terminator,
        /// to the output sink.
        /// </summary>
        /// <param name="error">The error to write.</param>
        void WriteError(string error);

        /// <summary>
        /// Writes the specified <paramref name="confirmation"/>,
        /// followed by the current line terminator,
        /// to the output sink.
        /// </summary>
        /// <param name="confirmation">The confirmation to write.</param>
        void WriteConfirmation(string confirmation);
    }
}
