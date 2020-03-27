namespace Minsk.Core.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        // Statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclarationStatement,
        IfStatement,

        // Expressions
        UnaryExpression,
        BinaryExpression,
        LiteralExpression,
        VariableExpression,
        AssignmentExpression
    }
}