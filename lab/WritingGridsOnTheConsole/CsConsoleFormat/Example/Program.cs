using System;
using System.Linq;
using System.Text;
using Alba.CsConsoleFormat;
using static System.ConsoleColor;

namespace Example
{
    internal class Program
    {
        private static void Main()
        {
            bool isConsoleAvailable = IsConsoleAvailable();

            Console.WriteLine($"Is console available: {isConsoleAvailable}");

            // Standard behavior if the console is available.
            Action initializeConsole = () => Console.OutputEncoding = Encoding.UTF8;
            Func<Rect?> getRenderRect = () => null;
            Action parkCursor = () => ConsoleRenderer.ConsoleCursorPosition = new Point(0, ConsoleRenderer.ConsoleCursorPosition.Y);

            if (!isConsoleAvailable)
            {
                initializeConsole = () => { };
                getRenderRect = () => new Rect(0, 0, 72, 100);
                parkCursor = () => { };
            }

            initializeConsole();

            var seeds = new[]
            {
                new
                {
                    Name = "This is my first seed",
                    Description = "This is some very long description of my first seed. A really very, very long description.",
                    CreatedEntities = "First\nSecond\nThird",
                    HasErrors = false
                },
                new
                {
                    Name = "This is my second seed",
                    Description = "This is some very long description of my second seed. A really very, very long description.",
                    CreatedEntities = "First\nSecond",
                    HasErrors = true
                },
                new
                {
                    Name = "Short",
                    Description = "This is some short description.",
                    CreatedEntities = "First",
                    HasErrors = true
                },
                new
                {
                    Name = "Also short",
                    Description = "This is some short description but a bit longer",
                    CreatedEntities = "",
                    HasErrors = false
                },
                new
                {
                    Name = "Short",
                    Description = "",
                    CreatedEntities = "SomeVeryLongNonWrappableEntityNameButReallyVeryVeryVeryVeryVeryLooooong",
                    HasErrors = false
                },
            };

            ConsoleRenderer.RenderDocument(GenerateDocument("Seeds"), null, getRenderRect());

            Console.WriteLine();

            ConsoleRenderer.RenderDocument(GenerateDocument("Scenarios"), null, getRenderRect());

            Console.WriteLine();

            ConsoleRenderer.RenderDocument(GenerateDocument("Something"), null, getRenderRect());

            Console.WriteLine();

            parkCursor();

            bool IsConsoleAvailable()
            {
                // Check for a better way to figure out if the direct access
                // to the console is available. Below are the two possibilities
                // that came to my mind.

                bool isAvailableFirstPossibility = !(Console.IsErrorRedirected || Console.IsInputRedirected || Console.IsOutputRedirected);

                bool isAvailableSecondPossibility = true;
                try
                {
                    // We just have to touch any of the properties like e.g. Console.BufferWidth or any other.                    
                    Console.CursorLeft = Console.CursorLeft;
                }
                catch
                {
                    isAvailableSecondPossibility = false;
                }

                return isAvailableSecondPossibility;
            }

            Document GenerateDocument(string title)
            {
                var lst = new LineThickness(LineWidth.None);

                var headerDocument = new Document(
                    new Span(title)
                    )
                {
                    TextAlign = TextAlign.Center,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0)
                };

                var doc = new Document(
                    headerDocument,
                    "\n",
                    new Grid
                    {
                        Margin = new Thickness(2, 0, 2, 0),

                        Columns =
                        {
                        new Column { Width = GridLength.Star(3) },
                        new Column { Width = GridLength.Star(4) },
                        new Column { Width = GridLength.Star(6) }
                        },

                        Children = {
                        new Cell("Name") { TextWrap = TextWrap.WordWrap },
                        new Cell("Description") { TextWrap = TextWrap.WordWrap },
                        new Cell("Creates entities") { TextWrap = TextWrap.WordWrap },
                        seeds.Select(seed => new[] {
                            new Cell {
                                Color = seed.HasErrors ? Red : Gray,
                                Children = { seed.Name }
                            },
                            new Cell {
                                Color = seed.HasErrors ? Red : Gray,
                                Children = { seed.Description }
                            },
                            new Cell {
                                Color = seed.HasErrors ? Red : Gray,
                                Children = { seed.CreatedEntities }
                            }
                        })
                        }
                    }
                )
                {
                    Margin = new Thickness(0),
                    Padding = new Thickness(0)
                };

                return doc;
            }
        }        
    }
}
