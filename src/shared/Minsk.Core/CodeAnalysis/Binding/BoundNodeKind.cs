namespace Minsk.Core.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        // Statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclarationStatement,
        IfStatement,
        WhileStatement,

        // Expressions
        UnaryExpression,
        BinaryExpression,
        LiteralExpression,
        VariableExpression,
        AssignmentExpression
    }
}