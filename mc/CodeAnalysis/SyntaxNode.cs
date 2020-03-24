using System.Collections.Generic;

namespace mc.CodeAnalysis
{
    internal abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}