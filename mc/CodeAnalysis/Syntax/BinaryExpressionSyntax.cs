using System.Collections.Generic;

namespace mc.CodeAnalysis.Syntax
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
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }

        public ExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Right { get; }
    }
}