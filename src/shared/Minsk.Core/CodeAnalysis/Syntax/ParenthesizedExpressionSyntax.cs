namespace Minsk.Core.CodeAnalysis.Syntax
{
    public sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public ParenthesizedExpressionSyntax(SyntaxToken openParensToken, ExpressionSyntax expression, SyntaxToken closeParensToken)
        {
            OpenParensToken = openParensToken;
            Expression = expression;
            CloseParensToken = closeParensToken;
        }

        public SyntaxToken OpenParensToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseParensToken { get; }

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;
    }
}