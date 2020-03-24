namespace mc.CodeAnalysis.Syntax
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
        BangToken,
        AmpersandAmpersandToken,
        PipePipeToken,
        Identifier,

        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpression,
        UnaryExpression,

        // Keywords
        FalseKeyword,
        TrueKeyword
    }
}