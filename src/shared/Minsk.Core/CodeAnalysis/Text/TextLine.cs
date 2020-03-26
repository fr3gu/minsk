namespace Minsk.Core.CodeAnalysis.Text
{
    public sealed class TextLine
    {
        public SourceText Text { get; }
        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;
        public int LengthInclLineBreak { get; }
        public TextSpan Span => new TextSpan(Start, Length);
        public TextSpan SpanInclLineBreak => new TextSpan(Start, LengthInclLineBreak);
        public override string ToString() => Text.ToString(Span);

        public TextLine(SourceText text, int start, int length, int lengthInclLineBreak)
        {
            Text = text;
            Start = start;
            Length = length;
            LengthInclLineBreak = lengthInclLineBreak;
        }
    }
}