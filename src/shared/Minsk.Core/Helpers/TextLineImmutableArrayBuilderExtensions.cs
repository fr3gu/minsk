using System.Collections.Immutable;
using Minsk.Core.CodeAnalysis.Text;

namespace Minsk.Core.Helpers
{
    internal static class TextLineImmutableArrayBuilderExtensions
    {
        public static void AddLine(this ImmutableArray<TextLine>.Builder result, SourceText sourceText, int position, int lineStart, int lineBreakWidth)
        {
            var lineLength = position - lineStart;
            var lengthInclLineBreak = lineLength + lineBreakWidth;
            var line = new TextLine(sourceText, lineStart, lineLength, lengthInclLineBreak);
            result.Add(line);
        }
    }
}