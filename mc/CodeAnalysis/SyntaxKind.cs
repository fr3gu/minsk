namespace mc.CodeAnalysis
{
    internal enum SyntaxKind
    {
        NumberToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParensToken,
        CloseParensToken,
        BadToken,
        EofToken,
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression
    }
}