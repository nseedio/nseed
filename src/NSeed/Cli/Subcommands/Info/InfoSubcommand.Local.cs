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
        // We have a different rendering behavior if the
        // physical console is available and if it is not.
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
            System.Diagnostics.Debug.Assert(seedBucket != null);
            System.Diagnostics.Debug.Assert(output != null);
            System.Diagnostics.Debug.Assert(textColorsProvider != null);

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

            ConsoleRenderer.RenderDocument(GenerateSummary(), null, renderingBehavior.RenderRect);

            if (numberOfSeeds > 0)
            {
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine($"Seeds");
                Console.WriteLine($"=====");
                int counter = 0;
                foreach (var seed in seedBucketInfo.ContainedSeedables.OfType<SeedInfo>())
                {
                    Console.WriteLine($"Name:             {seed.FriendlyName}");
                    Console.WriteLine($"Description:      {seed.Description}");
                    Console.WriteLine($"Creates entities: {string.Join(Environment.NewLine, seed.YieldedEntities.Select(entity => entity.FullName))}");
                    if (++counter != numberOfSeeds) Console.WriteLine("-----");
                }
            }

            if (numberOfScenarios > 0)
            {
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine($"Scenarios");
                Console.WriteLine($"=========");
                int counter = 0;
                foreach (var scenario in seedBucketInfo.ContainedSeedables.OfType<ScenarioInfo>())
                {
                    Console.WriteLine($"Name:             {scenario.FriendlyName}");
                    Console.WriteLine($"Description:      {scenario.Description}");
                    if (++counter != numberOfScenarios) Console.WriteLine("-----");
                }
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
                            new Cell("Name:") { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap },
                            new Cell(seedBucketInfo.FriendlyName) { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap, Padding = new Thickness(1, 0, 1, 0) },
                            new Cell("Description:") { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap },
                            new Cell(seedBucketInfo.Description) { Stroke = LineThickness.None, TextWrap = TextWrap.WordWrap, Padding = new Thickness(1, 0, 1, 0) },
                            new Cell("Number of seeds:") { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap },
                            new Cell(numberOfSeeds) { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap, Padding = new Thickness(1, 0, 1, 0) },
                            new Cell("Number of scenarios:") { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap },
                            new Cell(numberOfScenarios) { Stroke = LineThickness.None, TextWrap = TextWrap.NoWrap, Padding = new Thickness(1, 0, 1, 0) },
                        }
                    }
                )
                {
                    Color = textColors.Message,
                    Background = textColors.Background
                };
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
