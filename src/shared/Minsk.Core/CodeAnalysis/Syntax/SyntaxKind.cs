namespace Minsk.Core.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        BadToken,
        EofToken,
        WhitespaceToken,
        NumberToken,
        StringToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParensToken,
        CloseParensToken,
        OpenBraceToken,
        CloseBraceToken,
        BangToken,
        AmpersandToken,
        AmpersandAmpersandToken,
        PipeToken,
        PipePipeToken,
        EqualsEqualsToken,
        BangEqualsToken,
        EqualsToken,
        IdentifierToken,
        LessThanToken,
        LessThanOrEqualToken,
        GreaterThanToken,
        GreaterThanOrEqualToken,
        HatToken,
        TildeToken,

        // Keywords
        FalseKeyword,
        TrueKeyword,
        LetKeyword,
        VarKeyword,
        IfKeyword,
        ElseKeyword,
        WhileKeyword,
        ForKeyword,
        ToKeyword,

        // Nodes
        CompilationUnit,
        ElseClause,

        // Statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclarationStatement,
        IfStatement,
        WhileStatement,
        ForStatement,

        // Expressions
        ParenthesizedExpression,
        LiteralExpression,
        NameExpression,
        BinaryExpression,
        UnaryExpression,
        AssignmentExpression
    }
}