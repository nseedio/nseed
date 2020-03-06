using System;

namespace NSeed.Cli
{
    internal static class ConsoleUtil
    {
        public static bool IsPhysicalConsoleAvailable()
        {
            bool outputIsNotRedirected = !(Console.IsErrorRedirected || Console.IsInputRedirected || Console.IsOutputRedirected);

            bool physicalConsoleExists = true;
            try
            {
                // We just have to touch any of the properties like e.g. Console.BufferWidth, Console.CursorLeft
                // or any other that have to do something with the console buffer and the cursor.
                Console.CursorLeft = Console.CursorLeft;
            }
            catch
            {
                physicalConsoleExists = false;
            }

            // I guess these two checks are redundant.
            // I guess we cannot not have a console without redirection in place.
            // I guess. Only. Therefore, the double check :-)
            return outputIsNotRedirected && physicalConsoleExists;
        }
    }
}
