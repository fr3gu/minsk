using System;

namespace Minsk.Core.CodeAnalysis.Binding
{
    internal sealed class BoundAssignmentExpression : BoundExpression
    {
        public BoundAssignmentExpression(VariableSymbol symbol, BoundExpression expression)
        {
            Symbol = symbol;
            Expression = expression;
        }


        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public override Type Type => Expression.Type;
        public VariableSymbol Symbol { get; }
        public BoundExpression Expression { get; }
    }
}