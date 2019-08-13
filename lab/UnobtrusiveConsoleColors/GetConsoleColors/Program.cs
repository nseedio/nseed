using System;

namespace GetConsoleColors
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"Change the console colors and press any key to get the {nameof(ConsoleColor)} colors.");


            ConsoleColor? calculatedColor = null;
            while (true)
            {
                Console.WriteLine($"Background color:            {Console.BackgroundColor}");
                Console.WriteLine($"Foreground color:            {Console.ForegroundColor}");
                Console.WriteLine($"Calculated foreground color: {calculatedColor}");

                Console.Write("Get RGB (e.g. 255 0 123) or press enter to continue: ");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    var rgb = input.Split(' ');
                    if (rgb.Length != 3) continue;

                    var red = byte.Parse(rgb[0]);
                    var green = byte.Parse(rgb[1]);
                    var blue = byte.Parse(rgb[2]);

                    calculatedColor = ConsoleColorFromRgb(red, green, blue);
                }
                else
                {
                    calculatedColor = null;
                }
            }

            // Algorithm taken from: https://www.jerriepelser.com/blog/determine-consolecolor-from-hex-color/
            static ConsoleColor ConsoleColorFromRgb(byte red, byte green, byte blue)
            {
                int index = (red > 128 | green > 128 | blue > 128) ? 8 : 0; // Bright bit
                index |= (red > 64) ? 4 : 0; // Red bit
                index |= (green > 64) ? 2 : 0; // Green bit
                index |= (blue > 64) ? 1 : 0; // Blue bit

                return (ConsoleColor)index;
            }
        }
    }
}
