using System;

namespace NSeed.Cli
{
    internal class TextColors
    {
        public ConsoleColor Background { get; }

        public ConsoleColor Normal { get; }

        public ConsoleColor Warning { get; }

        public ConsoleColor Error { get; }

        public ConsoleColor Confirmation { get; }

        public TextColors(TextColorsTheme textColorsTheme, ConsoleColor foregroundColor, ConsoleColor backgroundColor, bool isNoColor)
        {
            System.Diagnostics.Debug.Assert(textColorsTheme != null);

            Background = backgroundColor;
            Normal = foregroundColor;
            Warning = foregroundColor;
            Error = foregroundColor;
            Confirmation = foregroundColor;

            if (isNoColor) return;

            Warning = textColorsTheme.GetWarningColorFor(foregroundColor, backgroundColor);
            Error = textColorsTheme.GetErrorColorFor(foregroundColor, backgroundColor);
            Confirmation = textColorsTheme.GetConfirmationColorFor(foregroundColor, backgroundColor);
        }
    }
}
