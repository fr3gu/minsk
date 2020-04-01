using System;
using Minsk.Core.CodeAnalysis.Symbols;

namespace Minsk.Core.CodeAnalysis.Binding
{
    internal sealed class BoundLiteralExpression : BoundExpression
    {
        public BoundLiteralExpression(object value)
        {
            Value = value;

            switch (value)
            {
                case bool _:
                    Type = TypeSymbol.Bool;
                    break;
                case int _:
                    Type = TypeSymbol.Int;
                    break;
                case string _:
                    Type = TypeSymbol.String;
                    break;
                default:
                    throw new Exception($"Type unsupported <{value.GetType()}>");
            }
        }

        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public override TypeSymbol Type { get; }
        public object Value { get; }
    }
}