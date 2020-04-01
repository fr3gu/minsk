using System;
using Minsk.Core.CodeAnalysis.Symbols;

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
        public override TypeSymbol Type => Expression.Type;
        public VariableSymbol Symbol { get; }
        public BoundExpression Expression { get; }
    }

    internal sealed class BoundErrorExpression : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.ErrorExpression;
        public override TypeSymbol Type => TypeSymbol.Error;
    }
}