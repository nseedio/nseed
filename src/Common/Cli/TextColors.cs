using System;

namespace NSeed.Cli
{
    internal class TextColors
    {
        public ConsoleColor Background { get; }

        public ConsoleColor Message { get; }

        public ConsoleColor Warning { get; }

        public ConsoleColor Error { get; }

        public ConsoleColor Confirmation { get; }

        public TextColors(TextColorsTheme textColorsTheme, ConsoleColor foregroundColor, ConsoleColor backgroundColor, bool noColor)
        {
            System.Diagnostics.Debug.Assert(textColorsTheme != null);

            Background = backgroundColor;
            Message = foregroundColor;
            Warning = foregroundColor;
            Error = foregroundColor;
            Confirmation = foregroundColor;

            if (noColor) return;

            if (textColorsTheme != null)
            {
                Warning = textColorsTheme.GetWarningColorFor(foregroundColor, backgroundColor);
                Error = textColorsTheme.GetErrorColorFor(foregroundColor, backgroundColor);
                Confirmation = textColorsTheme.GetConfirmationColorFor(foregroundColor, backgroundColor);
            }
        }
    }
}
