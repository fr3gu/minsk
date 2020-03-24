using System.Collections.Generic;

namespace Minsk.Core.CodeAnalysis.Syntax
{

    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private int _position;

        public Parser(string text)
        {
            Diagnostics = new List<string>();
            var lexer = new Lexer(text);
            SyntaxToken token;

            var tokens = new List<SyntaxToken>();

            do
            {
                token = lexer.Lex();

                if (token.Kind != SyntaxKind.WhitespaceToken && token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }

            } while (token.Kind != SyntaxKind.EofToken);

            _tokens = tokens.ToArray();
            Diagnostics.AddRange(lexer.Diagnostics);
        }

        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;

            return index >= _tokens.Length ? _tokens[^1] : _tokens[index];
        }

        private SyntaxToken Current => Peek(0);
        public List<string> Diagnostics { get; }

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
            {
                return NextToken();
            }

            Diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}> on position {_position}, expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            var expression = ParseExpression();
            var eofToken = MatchToken(SyntaxKind.EofToken);

            return new SyntaxTree(Diagnostics, expression, eofToken);
        }

        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();

                if (precedence == 0 || precedence <= parentPrecedence) break;

                var operatorToken = NextToken();

                var right = ParseExpression(precedence);

                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OpenParensToken:
                {
                    var left = NextToken();
                    var expression = ParseExpression();
                    var right = MatchToken(SyntaxKind.CloseParensToken);

                    return new ParenthesizedExpressionSyntax(left, expression, right);
                }
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                    var keywordToken = NextToken();
                    var value = keywordToken.Kind == SyntaxKind.TrueKeyword;
                    return new LiteralExpressionSyntax(keywordToken, value);
                default:
                    var numberToken = MatchToken(SyntaxKind.NumberToken);
                    return new LiteralExpressionSyntax(numberToken);
            }

        }
    }
}