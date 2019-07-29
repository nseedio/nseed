// This file isn't generated, but this comment is necessary to exclude it from StyleCop analysis.
// For more info see: https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2108
// <auto-generated/>

using JetBrains.Annotations;

namespace Alba.CsConsoleFormat.Fluent
{
    internal static class DocumentExts
    {
        public static void Render([NotNull] this Document @this, [CanBeNull] IRenderTarget target = null, Rect? renderRect = null) =>
            ConsoleRenderer.RenderDocument(@this, target, renderRect);

        public static string RenderToText([NotNull] this Document document, [NotNull] TextRenderTargetBase target, Rect? renderRect = null) =>
            ConsoleRenderer.RenderDocumentToText(document, target, renderRect);

        public static void RenderToBuffer([NotNull] this Document document, [NotNull] ConsoleBuffer buffer, Rect renderRect) =>
            ConsoleRenderer.RenderDocumentToBuffer(document, buffer, renderRect);
    }
}