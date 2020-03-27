namespace Minsk.Core.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EofToken,
        WhitespaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParensToken,
        CloseParensToken,
        OpenBraceToken,
        CloseBraceToken,
        BangToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        EqualsEqualsToken,
        BangEqualsToken,
        EqualsToken,
        IdentifierToken,
        LessThanToken,
        LessThanOrEqualToken,
        GreaterThanToken,
        GreaterThanOrEqualToken,

        // Keywords
        FalseKeyword,
        TrueKeyword,
        LetKeyword,
        VarKeyword,
        IfKeyword,
        ElseKeyword,
        WhileKeyword,

        // Nodes
        CompilationUnit,
        ElseClause,

        // Statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclarationStatement,
        IfStatement,
        WhileStatement,

        // Expressions
        ParenthesizedExpression,
        LiteralExpression,
        NameExpression,
        BinaryExpression,
        UnaryExpression,
        AssignmentExpression
    }
}