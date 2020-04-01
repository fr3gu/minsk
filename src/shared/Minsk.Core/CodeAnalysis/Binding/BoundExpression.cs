using System;
using Minsk.Core.CodeAnalysis.Symbols;

namespace Minsk.Core.CodeAnalysis.Binding
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract TypeSymbol Type { get; }
    }
}