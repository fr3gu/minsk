namespace Minsk.Core.CodeAnalysis.Syntax
{
    // 1 + 2 + 3
    //
    //      +
    //     / \
    //    +   3
    //   / \
    //  1   2
    //

    // 1 + 2 * 3
    //
    //    +
    //   / \
    //  1   *
    //     / \
    //    2   3
    //

    public sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        public ExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Right { get; }
    }
}