using System.Collections.Generic;
using System.Linq;

namespace Minsk.Core.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<Diagnostic> diagnostics, ExpressionSyntax root, SyntaxToken eofToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EofToken = eofToken;
        }

        public IReadOnlyList<Diagnostic> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EofToken { get; }

        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }

        public static IEnumerable<SyntaxToken> ParseTokens(string text)
        {
            var lexer = new Lexer(text);
            while (true)
            {
                var token = lexer.Lex();
                if (token.Kind == SyntaxKind.EofToken) break;

                yield return token;
            }
        }
    }
}