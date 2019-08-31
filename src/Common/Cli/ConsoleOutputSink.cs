using NSeed.Abstractions;
using System;

namespace NSeed.Cli
{
    internal class ConsoleOutputSink : IOutputSink, ITextColorsProvider
    {
        private readonly TextColors textColors;
        private readonly bool acceptsVerboseMessages;

        public ConsoleOutputSink(TextColors textColors, bool acceptsVerboseMessages)
        {
            System.Diagnostics.Debug.Assert(textColors != null);

            this.textColors = textColors ?? new TextColors(
                TextColorsTheme.GetForCurrentOS(),
                ConsoleColor.Gray,
                ConsoleColor.Black,
                false);

            this.acceptsVerboseMessages = acceptsVerboseMessages;
        }

        public static ConsoleOutputSink Create(bool noColor, bool acceptsVerboseMessages)
        {
            var textColorsTheme = TextColorsTheme.GetForCurrentOS();

            var (foregroundColor, backgroundColor) = GetForegroundAndBackgroundColor();

            var textColors = new TextColors(textColorsTheme, foregroundColor, backgroundColor, noColor);

            return new ConsoleOutputSink(textColors, acceptsVerboseMessages);

            static (ConsoleColor, ConsoleColor) GetForegroundAndBackgroundColor()
            {
                // The defaults, if the physical console is not available,
                // are actually irrelevant, so let's go with the Windows defaults.
                var foregroundColor = ConsoleColor.Gray;
                var backgroundColor = ConsoleColor.Black;

                if (ConsoleUtil.IsPhysicalConsoleAvailable())
                {
                    foregroundColor = Console.ForegroundColor;
                    backgroundColor = Console.BackgroundColor;
                }

                return (foregroundColor, backgroundColor);
            }
        }

        /// <summary>
        /// Shows the error message that happens within the initialization phase
        /// before the <see cref="IOutputSink"/> service is created.
        /// <br/>
        /// Never use this method to display error messages outside of the
        /// initialization code.
        /// </summary>
        /// <remarks>
        /// This method internally safely creates an <see cref="IOutputSink"/>
        /// and uses its <see cref="IOutputSink.WriteError(string)"/> method.
        /// </remarks>
        /// <param name="errorMessage">The error message to show.</param>
        internal static void ShowInitializationErrorMessage(string errorMessage)
        {
            IOutputSink output = Create(noColor: false, acceptsVerboseMessages: false);
            output.WriteError(errorMessage);
        }

        bool IOutputSink.AcceptsVerboseMessages => acceptsVerboseMessages;

        void IOutputSink.WriteLine() => Console.WriteLine();

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

        TextColors ITextColorsProvider.GetTextColors() => textColors;

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
