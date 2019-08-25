using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Alba.CsConsoleFormat;
using static System.ConsoleColor;

namespace NSeed
{
    public class SeedBucket
    {
        public static void Handle<TSeedBucket>(string[] commandLineArguments)
            where TSeedBucket : SeedBucket
        {
            bool isConsoleAvailable = IsPhysicalConsoleAvailable();

            Console.WriteLine($"Is console available: {isConsoleAvailable}");

            // Standard behavior if the console is available.
            Action initializeConsole = () =>
            {
                // According to CsConsoleFormat docummentation,
                // if OS is Windows we have to set encoding to UTF8.
                // See: https://github.com/Athari/CsConsoleFormat#getting-started
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Console.OutputEncoding = Encoding.UTF8;
            };
            Rect? renderRect = new Rect(0, 0, Math.Min(Console.BufferWidth, 150), Size.Infinity);
            var headerStroke = new LineThickness(LineWidth.None, LineWidth.None, LineWidth.None, LineWidth.Single);
            bool underlineGridHeader = false;
            bool writeFinalLineTerminator = true;
            var foregroundColor = Console.ForegroundColor;
            var backgroundColor = Console.BackgroundColor;

            if (!isConsoleAvailable)
            {
                initializeConsole = () => { };
                renderRect = new Rect(0, 0, 72, Size.Infinity);
                headerStroke = LineThickness.None;
                underlineGridHeader = true;
                writeFinalLineTerminator = false;
                foregroundColor = Gray;
                backgroundColor = Black;
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

            Console.WriteLine();
            Console.WriteLine();

            ConsoleRenderer.RenderDocument(GenerateSummary(), null, renderRect);

            ConsoleRenderer.RenderDocument(GenerateSeeds(), null, renderRect);

            ConsoleRenderer.RenderDocument(GenerateScenarios(), null, renderRect);

            if (writeFinalLineTerminator) Console.WriteLine();

            bool IsPhysicalConsoleAvailable()
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

            Grid CreateHeaderGrid(string text)
            {
                return new Grid
                {
                    Stroke = LineThickness.None,

                    Columns =
                    {
                        new Column { Width = GridLength.Auto }
                    },

                    Children =
                    {
                        new Cell(text) { Stroke = headerStroke, TextWrap = TextWrap.NoWrap },
                        underlineGridHeader
                            ? new Cell(new string('=', text.Length)) { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap }
                            : null
                    }
                };
            }

            Document GenerateSummary()
            {
                var doc = new Document(
                    CreateHeaderGrid("Seed Bucket Summary"),
                    new Grid
                    {
                        Margin = new Thickness(1, 0, 1, 2),
                       
                        Stroke = LineThickness.None,
                        
                        Columns =
                        {
                            new Column { Width = GridLength.Auto },
                            new Column { Width = GridLength.Auto }
                        },

                        Children = {
                            new Cell("Name:") { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap },
                            new Cell("Some seed bucket name") { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap, Padding = new Thickness(1, 0, 1, 0) },
                            new Cell("Description:") { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap },
                            new Cell("Some seed bucket description that can potentially be very long.") { Stroke = LineThickness.None, TextWrap = TextWrap.WordWrap, Padding = new Thickness(1, 0, 1, 0) },
                            new Cell("Number of seeds:") { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap },
                            new Cell(12) { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap, Padding = new Thickness(1, 0, 1, 0) },
                            new Cell("Number of scenarios:") { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap },
                            new Cell(15) { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap, Padding = new Thickness(1, 0, 1, 0) },
                        }
                    }
                )
                {
                    Color = foregroundColor,
                    Background = backgroundColor
                };

                return doc;
            }

            Document GenerateSeeds()
            {
                var doc = new Document(
                    CreateHeaderGrid("Seeds"),
                    new Grid
                    {
                        Margin = new Thickness(1, 0, 1, 1),

                        Stroke = LineThickness.None,

                        Columns =
                        {
                            new Column { Width = GridLength.Star(3) },
                            new Column { Width = GridLength.Star(4) },
                            new Column { Width = GridLength.Star(3) }
                        },

                        Children = {
                            new Cell("Name") { Stroke = headerStroke, TextWrap = TextWrap.NoWrap },
                            new Cell("Description") { Stroke = headerStroke, TextWrap = TextWrap.NoWrap },
                            new Cell("Creates") { Stroke = headerStroke, TextWrap = TextWrap.NoWrap },
                            seeds.Select(seed => new[] {
                                new Cell {
                                    Padding = new Thickness(1, 0, 1, 1),
                                    Stroke = LineThickness.None,
                                    Color = seed.HasErrors ? Red : foregroundColor,
                                    Children = { seed.Name }
                                },
                                new Cell {
                                    Padding = new Thickness(1, 0, 1, 1),
                                    Stroke = LineThickness.None,
                                    Color = seed.HasErrors ? Red : foregroundColor,
                                    Children = { seed.Description }
                                },
                                new Cell {
                                    Padding = new Thickness(1, 0, 1, 1),
                                    Stroke = LineThickness.None,
                                    Color = seed.HasErrors ? Red : foregroundColor,
                                    Children = { seed.CreatedEntities }
                                }
                            })
                        }
                    }
                )
                {
                    Color = foregroundColor,
                    Background = backgroundColor
                };

                return doc;
            }

            Document GenerateScenarios()
            {
                var doc = new Document(
                    CreateHeaderGrid("Scenarios"),
                    new Grid
                    {
                        Margin = new Thickness(1, 0, 1, 0),

                        Stroke = LineThickness.None,

                        Columns =
                        {
                            new Column { Width = GridLength.Star(4) },
                            new Column { Width = GridLength.Star(6) },
                        },

                        Children = {
                            new Cell("Name") { Stroke = headerStroke, TextWrap = TextWrap.NoWrap },
                            new Cell("Description") { Stroke = headerStroke, TextWrap = TextWrap.NoWrap },
                            seeds.Select(seed => new[] {
                                new Cell {
                                    Padding = new Thickness(1, 0, 1, 1),
                                    Stroke = LineThickness.None,
                                    Color = seed.HasErrors ? Red : foregroundColor,
                                    Children = { seed.Name }
                                },
                                new Cell {
                                    Padding = new Thickness(1, 0, 1, 1),
                                    Stroke = LineThickness.None,
                                    Color = seed.HasErrors ? Red : foregroundColor,
                                    Children = { seed.Description }
                                }
                            })
                        }
                    }
                )
                {
                    Color = foregroundColor,
                    Background = backgroundColor
                };

                return doc;
            }
        }        
    }
}
