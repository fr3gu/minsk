namespace mc.CodeAnalysis
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
        // Expressions
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression
    }
}