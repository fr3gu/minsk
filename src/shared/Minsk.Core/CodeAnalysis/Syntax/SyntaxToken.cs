namespace Minsk.Core.CodeAnalysis.Syntax
{
    public class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }


        public int Position { get; }

        public string Text { get; }

        public object Value { get; }
        public override SyntaxKind Kind { get; }
        public override TextSpan Span => new TextSpan(Position, Text.Length);
    }
}