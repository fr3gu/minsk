using System.Collections.Generic;
using System.Linq;

namespace Minsk.Core.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken eofToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EofToken = eofToken;
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EofToken { get; }

        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
}