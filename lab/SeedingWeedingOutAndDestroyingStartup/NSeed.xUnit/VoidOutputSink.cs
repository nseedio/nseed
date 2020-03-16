using NSeed.Abstractions;

namespace NSeed.Xunit
{
    internal class VoidOutputSink : IOutputSink
    {
        public bool AcceptsVerboseMessages => true;

        public void WriteConfirmation(string confirmation)
        {
        }

        public void WriteError(string error)
        {
        }

        public void WriteLine()
        {
        }

        public void WriteMessage(string message)
        {
        }

        public void WriteVerboseMessage(string verboseMessage)
        {
        }

        public void WriteWarning(string warning)
        {
        }
    }
}
