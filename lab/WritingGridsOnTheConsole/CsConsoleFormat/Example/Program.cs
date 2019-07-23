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
            Console.OutputEncoding = Encoding.UTF8;

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


            ConsoleRenderer.RenderDocument(GenerateDocument("Seeds"));

            Console.WriteLine();

            ConsoleRenderer.RenderDocument(GenerateDocument("Scenarios"));

            Console.WriteLine();

            ConsoleRenderer.RenderDocument(GenerateDocument("Something"));

            Console.WriteLine();

            ConsoleRenderer.ConsoleCursorPosition = new Point(0, ConsoleRenderer.ConsoleCursorPosition.Y);

            Console.ReadKey(true);

            Document GenerateDocument(string title)
            {
                var headerThickness = new LineThickness(LineWidth.Double, LineWidth.Single);

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
