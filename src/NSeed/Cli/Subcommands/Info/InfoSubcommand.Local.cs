using Alba.CsConsoleFormat;
using NSeed.Abstractions;
using NSeed.MetaInfo;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands.Info
{
    internal partial class InfoSubcommand
    {
        // We have a different rendering behavior when the
        // physical console is available and when it is not.
        private class RenderingBehavior
        {
            public Action InitializeConsole { get; }

            public Rect RenderRect { get; }

            public LineThickness HeaderStroke { get; }

            public bool UnderlineGridHeader { get; }

            public bool WriteFinalLineTerminator { get; }

            private RenderingBehavior(Action initializeConsole, Rect renderRect, LineThickness headerStroke, bool underlineGridHeader, bool writeFinalLineTerminator)
            {
                InitializeConsole = initializeConsole;
                RenderRect = renderRect;
                HeaderStroke = headerStroke;
                UnderlineGridHeader = underlineGridHeader;
                WriteFinalLineTerminator = writeFinalLineTerminator;
            }

            public static RenderingBehavior Create()
            {
                // Standard behavior if the physical console is available.
                Action initializeConsole = () =>
                {
                    // According to CsConsoleFormat docummentation,
                    // if OS is Windows we have to set encoding to UTF8 if we want to
                    // use Unicode rendering, which we do.
                    // See: https://github.com/Athari/CsConsoleFormat#getting-started
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        Console.OutputEncoding = Encoding.UTF8;
                };

                // We limit the length to 150 characters, otherwise, if the
                // whole buffer is used on widescreen the output will be too
                // much spread out.
                var renderRect = new Rect(0, 0, Math.Min(Console.BufferWidth, 150), Size.Infinity);
                var headerStroke = new LineThickness(LineWidth.None, LineWidth.None, LineWidth.None, LineWidth.Single);
                bool underlineGridHeader = false;
                bool writeFinalLineTerminator = true;

                if (!ConsoleUtil.IsPhysicalConsoleAvailable())
                {
                    initializeConsole = () => { };
                    renderRect = new Rect(0, 0, 72, Size.Infinity);
                    headerStroke = LineThickness.None;
                    underlineGridHeader = true;
                    writeFinalLineTerminator = false;
                }

                return new RenderingBehavior(initializeConsole, renderRect, headerStroke, underlineGridHeader, writeFinalLineTerminator);
            }
        }

        private readonly SeedBucket seedBucket;
        private readonly IOutputSink output;
        private readonly TextColors textColors;
        private readonly RenderingBehavior renderingBehavior = RenderingBehavior.Create();

        public InfoSubcommand(SeedBucket seedBucket, IOutputSink output, ITextColorsProvider textColorsProvider)
        {
            this.seedBucket = seedBucket;
            this.output = output;
            textColors = textColorsProvider.GetTextColors();
        }

        public Task OnExecute()
        {
            output.WriteLine();

            var seedBucketInfo = seedBucket.GetMetaInfo();

            int numberOfSeeds = seedBucketInfo.ContainedSeedables.OfType<SeedInfo>().Count();
            int numberOfScenarios = seedBucketInfo.ContainedSeedables.OfType<ScenarioInfo>().Count();
            int numberOfErrors = seedBucketInfo.AllErrors.Count();

            ConsoleRenderer.RenderDocument(GenerateSummary(), null, renderingBehavior.RenderRect);

            if (numberOfSeeds > 0)
            {
                ConsoleRenderer.RenderDocument(GenerateSeedsInfo(), null, renderingBehavior.RenderRect);
            }

            if (numberOfScenarios > 0)
            {
                ConsoleRenderer.RenderDocument(GenerateScenariosInfo(), null, renderingBehavior.RenderRect);
            }

            output.WriteLine();

            return Task.CompletedTask;

            Document GenerateSummary()
            {
                return new Document(
                    CreateHeading("Seed Bucket Summary"),
                    new Grid
                    {
                        Margin = new Thickness(1, 0, 1, 2),

                        Stroke = LineThickness.None,

                        Columns =
                        {
                            new Column { Width = GridLength.Auto },
                            new Column { Width = GridLength.Auto }
                        },

                        Children =
                        {
                            CreateDescriptionColumnCell("Name"),
                            CreateValueColumnCell(seedBucketInfo.FriendlyName),
                            CreateDescriptionColumnCell("Description"),
                            CreateValueColumnCell(seedBucketInfo.Description, textWrap: TextWrap.WordWrap),
                            CreateDescriptionColumnCell("Number of seeds"),
                            CreateValueColumnCell(numberOfSeeds),
                            CreateDescriptionColumnCell("Number of scenarios"),
                            CreateValueColumnCell(numberOfScenarios),
                            numberOfErrors > 0
                                ? CreateDescriptionColumnCell("Number of errors")
                                : null,
                            numberOfErrors > 0
                                ? CreateValueColumnCell(numberOfErrors, textColors.Error)
                                : null
                        }
                    }
                )
                {
                    Color = textColors.Message,
                    Background = textColors.Background
                };

                static Cell CreateDescriptionColumnCell(string text)
                {
                    return new Cell($"{text}:") { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap };
                }

                static Cell CreateValueColumnCell<TValue>(TValue value, ConsoleColor? color = null, TextWrap textWrap = TextWrap.NoWrap)
                {
                    return new Cell(value)
                    {
                        Stroke = LineThickness.None,
                        TextWrap = textWrap,
                        Padding = new Thickness(1, 0, 1, 0),
                        Color = color
                    };
                }
            }

            Document GenerateSeedsInfo()
            {
                return new Document(
                    CreateHeading("Seeds"),
                    new Grid
                    {
                        Margin = new Thickness(1, 0, 1, 1),

                        Stroke = LineThickness.None,

                        Columns =
                        {
                            new Column { Width = GridLength.Star(3) },
                            new Column { Width = GridLength.Star(3) },
                            new Column { Width = GridLength.Star(4) }
                        },

                        Children =
                        {
                            CreateInfoGridHeaderCell("Name"),
                            CreateInfoGridHeaderCell("Creates"),
                            CreateInfoGridHeaderCell("Description"),
                            seedBucketInfo.ContainedSeedables
                                .OfType<SeedInfo>()
                                .OrderBy(seed => seed.FriendlyName)
                                .Select(seed =>
                                {
                                    var textColor = GetCellTextColor(seed);
                                    return new[]
                                    {
                                        CreateInfoGridValueCell(seed.FriendlyName, textColor),
                                        CreateInfoGridValueCell(string.Join(Environment.NewLine, seed.YieldedEntities.Select(entity => entity.FullName)), textColor),
                                        CreateInfoGridValueCell(seed.Description, textColor)
                                    };
                                })
                        }
                    }
                )
                {
                    Color = textColors.Message,
                    Background = textColors.Background
                };
            }

            Document GenerateScenariosInfo()
            {
                return new Document(
                    CreateHeading("Scenarios"),
                    new Grid
                    {
                        Margin = new Thickness(1, 0, 1, 0),

                        Stroke = LineThickness.None,

                        Columns =
                        {
                            new Column { Width = GridLength.Star(4) },
                            new Column { Width = GridLength.Star(6) }
                        },

                        Children =
                        {
                            CreateInfoGridHeaderCell("Name"),
                            CreateInfoGridHeaderCell("Description"),
                            seedBucketInfo.ContainedSeedables
                                .OfType<ScenarioInfo>()
                                .OrderBy(scenario => scenario.FriendlyName)
                                .Select(scenario =>
                                {
                                    var textColor = GetCellTextColor(scenario);
                                    return new[]
                                    {
                                        CreateInfoGridValueCell(scenario.FriendlyName, textColor),
                                        CreateInfoGridValueCell(scenario.Description, textColor)
                                    };
                                })
                        }
                    }
                )
                {
                    Color = textColors.Message,
                    Background = textColors.Background
                };
            }

            Cell CreateInfoGridHeaderCell(string text)
            {
                return new Cell(text) { Stroke = renderingBehavior.HeaderStroke, TextWrap = TextWrap.NoWrap };
            }

            static Cell CreateInfoGridValueCell<TValue>(TValue value, ConsoleColor? color)
            {
                return new Cell
                {
                    Padding = new Thickness(1, 0, 1, 1),
                    Stroke = LineThickness.None,
                    Children = { value },
                    Color = color
                };
            }

            ConsoleColor? GetCellTextColor(MetaInfo.MetaInfo metaInfo)
            {
                return metaInfo.AllErrors.Any()
                    ? textColors.Error
                    : (ConsoleColor?)null;
            }

            Grid CreateHeading(string headingText)
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
                        new Cell(headingText) { Stroke = renderingBehavior.HeaderStroke, TextWrap = TextWrap.NoWrap },
                        renderingBehavior.UnderlineGridHeader
                            ? new Cell(new string('=', headingText.Length)) { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap }
                            : null
                    }
                };
            }
        }
    }
}
