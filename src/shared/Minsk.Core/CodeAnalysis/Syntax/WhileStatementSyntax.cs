namespace Minsk.Core.CodeAnalysis.Syntax
{
    internal class WhileStatementSyntax : StatementSyntax
    {
        public SyntaxToken WhileKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public StatementSyntax WhileStatement { get; }

        public WhileStatementSyntax(SyntaxToken whileKeyword, ExpressionSyntax condition, StatementSyntax whileStatement)
        {
            WhileKeyword = whileKeyword;
            Condition = condition;
            WhileStatement = whileStatement;
        }

        public override SyntaxKind Kind => SyntaxKind.WhileStatement;
    }
}