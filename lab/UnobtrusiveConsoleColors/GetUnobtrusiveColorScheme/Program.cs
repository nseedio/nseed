using System;

namespace GetUnobtrusiveColorScheme
{
    internal class TextColors
    {
        public ConsoleColor? ErrorColor { get; }
        public ConsoleColor? WarningColor { get; }
        public ConsoleColor? ConfirmationColor { get; }

        public TextColors(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            if (foregroundColor != ConsoleColor.Red && !IsRedUnfriendly(backgroundColor))
                ErrorColor = ConsoleColor.Red;

            if (foregroundColor != ConsoleColor.Yellow && !IsYellowUnfriendly(backgroundColor))
                WarningColor = ConsoleColor.Yellow;

            if (foregroundColor != ConsoleColor.Green && !IsGreenUnfriendly(backgroundColor))
                ConfirmationColor = ConsoleColor.Green;

            static bool IsRedUnfriendly(ConsoleColor color)
            {
                return
                    color == ConsoleColor.Red ||
                    color == ConsoleColor.DarkRed ||
                    color == ConsoleColor.Magenta;
            }

            static bool IsYellowUnfriendly(ConsoleColor color)
            {
                return
                    color == ConsoleColor.Yellow ||
                    color == ConsoleColor.White;
            }

            static bool IsGreenUnfriendly(ConsoleColor color)
            {
                return
                    color == ConsoleColor.DarkGreen ||
                    color == ConsoleColor.Green ||
                    color == ConsoleColor.DarkCyan ||                    
                    color == ConsoleColor.Cyan ||
                    color == ConsoleColor.DarkYellow;
            }
        }
    }

    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.Clear();

            for (int i = 0; i < 16; i++)
            {
                string consoleColorName = ((ConsoleColor)i).ToString();
                consoleColorName = consoleColorName.Substring(consoleColorName.Length - Math.Min(consoleColorName.Length, 6));
                Console.Write(consoleColorName.PadRight(6) + " ");
            }

            for (int foregroundColorIndex = 0; foregroundColorIndex < 16; foregroundColorIndex++)
            {                
                for (int backgroundColorIndex = 0; backgroundColorIndex < 16; backgroundColorIndex++)
                {
                    var foregroundColor = (ConsoleColor)foregroundColorIndex;
                    var backgroundColor = (ConsoleColor)backgroundColorIndex;

                    var colors = new TextColors(foregroundColor, backgroundColor);

                    Console.SetCursorPosition(backgroundColorIndex*7, foregroundColorIndex + 2);

                    if (foregroundColorIndex == backgroundColorIndex)
                    {
                        Console.ResetColor();
                        Console.WriteLine("       ");
                        continue;
                    }

                    WriteInColor(foregroundColor, backgroundColor, ' ');
                    WriteInColor(foregroundColor, backgroundColor, 'N');
                    WriteInColor(colors.WarningColor ?? foregroundColor, backgroundColor, 'W');
                    WriteInColor(colors.ErrorColor ?? foregroundColor, backgroundColor, 'E');
                    WriteInColor(colors.ConfirmationColor ?? foregroundColor, backgroundColor, 'C');
                    WriteInColor(foregroundColor, backgroundColor, ' ');

                    Console.ResetColor();
                    Console.Write(" ");

                    static void WriteInColor(ConsoleColor foregroundColor, ConsoleColor backgroundColor, char @char)
                    {
                        Console.ForegroundColor = foregroundColor;
                        Console.BackgroundColor = backgroundColor;
                        Console.Write(@char);
                        Console.ResetColor();
                    }
                }
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
