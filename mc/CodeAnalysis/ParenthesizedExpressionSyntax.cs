using System.Collections.Generic;

namespace mc.CodeAnalysis
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
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParensToken;
            yield return Expression;
            yield return CloseParensToken;
        }
    }
}