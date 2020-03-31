namespace Minsk.Core.CodeAnalysis.Binding
{
    internal enum BoundBinaryOperatorKind
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        LogicalAndAlso,
        LogicalOrElse,
        BitwiseAnd,
        BitwiseOr,
        BitwiseXOr,
        Equals,
        NotEquals,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual
    }
}