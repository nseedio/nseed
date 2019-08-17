using System;
using System.Runtime.InteropServices;
using static System.ConsoleColor;

namespace NSeed.Cli
{
    // I miss those discriminated unions soooooo much.
    internal class TextColorsTheme
    {
        // Standard color theme for Windows and Linux.
        // Taken from: https://github.com/deinsoftware/colorify/blob/master/Colorify/Theme/ThemeDark.cs
        private static readonly TextColorsTheme Dark = new TextColorsTheme(Yellow, Red, DarkGreen);

        // Standard color theme for Mac.
        // Taken from: https://github.com/deinsoftware/colorify/blob/master/Colorify/Theme/ThemeLight.cs
        private static readonly TextColorsTheme Light = new TextColorsTheme(DarkYellow, DarkRed, DarkGreen);

        private readonly ConsoleColor warning;

        private readonly ConsoleColor error;

        private readonly ConsoleColor confirmation;

        private TextColorsTheme(ConsoleColor warning, ConsoleColor error, ConsoleColor confirmation)
        {
            this.warning = warning;
            this.error = error;
            this.confirmation = confirmation;
        }

        public static TextColorsTheme GetForCurrentOS()
        {
            return
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? Dark
                : Light;
        }

        // The implementation of the GetXYZColorFor() methods knows that
        // warning is always yelowish, error redish, and confirmation greenish.

        public ConsoleColor GetWarningColorFor(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            return foregroundColor != warning && !IsYellowUnfriendly(backgroundColor)
                ? warning
                : foregroundColor;

            static bool IsYellowUnfriendly(ConsoleColor color)
            {
                return
                    color == Yellow ||
                    color == DarkYellow ||
                    color == White;
            }
        }

        public ConsoleColor GetErrorColorFor(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            return foregroundColor != error && !IsRedUnfriendly(backgroundColor)
                ? error
                : foregroundColor;

            static bool IsRedUnfriendly(ConsoleColor color)
            {
                return
                    color == Red ||
                    color == DarkRed ||
                    color == Magenta;
            }
        }

        public ConsoleColor GetConfirmationColorFor(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            return foregroundColor != confirmation && !IsGreenUnfriendly(backgroundColor)
                ? confirmation
                : foregroundColor;

            static bool IsGreenUnfriendly(ConsoleColor color)
            {
                return
                    color == DarkGreen ||
                    color == Green ||
                    color == DarkCyan ||
                    color == Cyan ||
                    color == DarkYellow;
            }
        }
    }
}
