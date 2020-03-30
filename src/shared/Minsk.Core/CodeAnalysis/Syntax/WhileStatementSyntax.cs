﻿namespace Minsk.Core.CodeAnalysis.Syntax
{
    internal class WhileStatementSyntax : StatementSyntax
    {
        public SyntaxToken WhileKeyword { get; }
        public ExpressionSyntax Condition { get; }
        public StatementSyntax Body { get; }

        public WhileStatementSyntax(SyntaxToken whileKeyword, ExpressionSyntax condition, StatementSyntax body)
        {
            WhileKeyword = whileKeyword;
            Condition = condition;
            Body = body;
        }

        public override SyntaxKind Kind => SyntaxKind.WhileStatement;
    }
}