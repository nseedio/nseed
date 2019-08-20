using NSeed.Abstractions;
using System;

namespace NSeed.Cli
{
    internal class ConsoleOutputSink : IOutputSink
    {
        private readonly TextColors textColors;
        private readonly bool acceptsVerboseMessages;

        public ConsoleOutputSink(TextColors textColors, bool acceptsVerboseMessages)
        {
            System.Diagnostics.Debug.Assert(textColors != null);

            this.textColors = textColors;
            this.acceptsVerboseMessages = acceptsVerboseMessages;
        }

        public static ConsoleOutputSink Create(bool noColor, bool acceptsVerboseMessages)
        {
            var textColorsTheme = TextColorsTheme.GetForCurrentOS();
            var textColors = new TextColors(textColorsTheme, Console.ForegroundColor, Console.BackgroundColor, noColor);

            return new ConsoleOutputSink(textColors, acceptsVerboseMessages);
        }

        bool IOutputSink.AcceptsVerboseMessages => acceptsVerboseMessages;

        void IOutputSink.WriteConfirmation(string confirmation)
        {
            WriteLineInColor(confirmation, textColors.Confirmation);
        }

        void IOutputSink.WriteError(string error)
        {
            WriteLineInColor(error, textColors.Error);
        }

        void IOutputSink.WriteMessage(string message)
        {
            WriteLineInColor(message, textColors.Message);
        }

        void IOutputSink.WriteVerboseMessage(string verboseMessage)
        {
            if (!acceptsVerboseMessages) return;

            WriteLineInColor(verboseMessage, textColors.Message);
        }

        void IOutputSink.WriteWarning(string warning)
        {
            WriteLineInColor(warning, textColors.Warning);
        }

        private void WriteLineInColor(string value, ConsoleColor color)
        {
            if (value == null) return;

            // Technically, we will never change the background.
            // This is here just in case users mess up with the console
            // in their own code. Very unlikely, but impossible
            // to forbid.
            Console.BackgroundColor = textColors.Background;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}
