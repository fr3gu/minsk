using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Minsk.Core.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(ImmutableArray<Diagnostic> diagnostics, ExpressionSyntax root, SyntaxToken eofToken)
        {
            Diagnostics = diagnostics;
            Root = root;
            EofToken = eofToken;
        }

        public ImmutableArray<Diagnostic> Diagnostics { get; }
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