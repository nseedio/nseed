using System;

namespace SetAndClearConsoleColors
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.WriteLine("Set the console foreground color to 255 255 255 and press any key when you are done.");

            Console.ReadKey(false);

            Console.WriteLine($"Background color:            {Console.BackgroundColor}");
            Console.WriteLine($"Foreground color:            {Console.ForegroundColor}");

            Console.WriteLine();

            Console.WriteLine("Some sample text");

            Console.WriteLine();

            Console.ForegroundColor = Console.ForegroundColor;

            Console.WriteLine();

            Console.WriteLine("Some sample text");

            Console.WriteLine();

            Console.ResetColor();

            Console.WriteLine();

            Console.WriteLine("Some sample text");

            Console.WriteLine();
        }
    }
}
