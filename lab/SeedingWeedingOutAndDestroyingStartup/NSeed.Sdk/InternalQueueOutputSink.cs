using NSeed.Abstractions;
using System.Collections.Generic;
using System.Text;

namespace NSeed.Sdk
{
    public class InternalQueueOutputSink : IOutputSink // TODO: Move to NSeed.
    {
        internal enum OutputKind
        {
            EmptyLine,
            Message,
            VerboseMessage,
            Warning,
            Error,
            Confirmation
        }

        internal class Output
        {
            public Output(OutputKind outputKind, string text)
            {
                OutputKind = outputKind;
                Text = text;
            }

            public OutputKind OutputKind { get; }

            public string Text { get; }

            public string AsString()
            {
                return GetOutputKindStringPrefix(OutputKind) + Text;

                static string GetOutputKindStringPrefix(OutputKind outputKind) =>
                    outputKind switch
                    {
                        OutputKind.EmptyLine => string.Empty,
                        OutputKind.Message => "MESSAGE     : ",
                        OutputKind.VerboseMessage => "VERBOSE     : ",
                        OutputKind.Warning => "WARNING     : ",
                        OutputKind.Error => "ERROR       : ",
                        OutputKind.Confirmation => "CONFIRMATION: ",
                        _ => string.Empty
                    };
            }
        }

        private readonly List<Output> outputs = new List<Output>();

        public bool AcceptsVerboseMessages => true;

        public void WriteConfirmation(string confirmation)
        {
            outputs.Add(new Output(OutputKind.Confirmation, confirmation));
        }

        public void WriteError(string error)
        {
             outputs.Add(new Output(OutputKind.Error, error));
        }

        public void WriteLine()
        {
              outputs.Add(new Output(OutputKind.EmptyLine, string.Empty));
        }

        public void WriteMessage(string message)
        {
              outputs.Add(new Output(OutputKind.Message, message));
        }

        public void WriteVerboseMessage(string verboseMessage)
        {
               outputs.Add(new Output(OutputKind.VerboseMessage, verboseMessage));
        }

        public void WriteWarning(string warning)
        {
                outputs.Add(new Output(OutputKind.Warning, warning));
        }

        public string GetOutputAsString()
        {
            var sb = new StringBuilder();

            foreach (var output in outputs)
            {
                sb.AppendLine(output.AsString());
            }

            return sb.ToString();
        }
    }
}
