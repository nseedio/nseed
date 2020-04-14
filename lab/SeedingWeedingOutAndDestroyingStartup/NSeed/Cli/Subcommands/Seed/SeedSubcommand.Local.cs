using Alba.CsConsoleFormat;
using NSeed.Abstractions;
using NSeed.Filtering;
using NSeed.MetaInfo;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Subcommands.Seed
{
    internal partial class SeedSubcommand
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
                if (ConsoleUtil.IsPhysicalConsoleAvailable())
                {
                    return new RenderingBehavior
                    (
                        initializeConsole: () =>
                        {
                            // According to CsConsoleFormat docummentation,
                            // if OS is Windows we have to set encoding to UTF8 if we want to
                            // use Unicode rendering, which we do.
                            // See: https://github.com/Athari/CsConsoleFormat#getting-started
                            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                                Console.OutputEncoding = Encoding.UTF8;
                        },
                        renderRect: new Rect(0, 0, Math.Min(Console.BufferWidth, 150), Size.Infinity),  // We limit the length to 150 characters, otherwise, if the whole buffer is used on widescreen the output will be too much spread out. We limit the length to 150 characters, otherwise, if the whole buffer is used on widescreen the output will be too much spread out. (BDW this comment is in one line because of a StyleCop bug :-()
                        headerStroke: new LineThickness(LineWidth.None, LineWidth.None, LineWidth.None, LineWidth.Single),
                        underlineGridHeader: false,
                        writeFinalLineTerminator: true
                    );
                }
                else
                {
                    return new RenderingBehavior
                    (
                        initializeConsole: () => { },
                        renderRect: new Rect(0, 0, 72, Size.Infinity),
                        headerStroke: LineThickness.None,
                        underlineGridHeader: true,
                        writeFinalLineTerminator: false
                    );
                }
            }
        }

        private readonly SeedBucket seedBucket;
        private readonly TextColors textColors;
        private readonly RenderingBehavior renderingBehavior = RenderingBehavior.Create();

        public SeedSubcommand(SeedBucket seedBucket, IOutputSink output, ITextColorsProvider textColorsProvider)
            : base(output)
        {
            this.seedBucket = seedBucket;
            textColors = textColorsProvider.GetTextColors();
        }

        public async Task OnExecute()
        {
            Output.WriteLine();
            var seedableFilter = !string.IsNullOrEmpty(Filter) ? new FullNameContainsSeedableFilter(Filter) : null;
            await seedBucket.Seed(Output, seedableFilter);
        }
    }
}
