using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

// ReSharper disable ArrangeTypeMemberModifiers
// ReSharper disable ArrangeTypeModifiers

namespace mc
{

    // 1 + 2 + 3
    //
    //      +
    //     / \
    //    +   3
    //   / \
    //  1   2
    //

    // 1 + 2 * 3
    //
    //    +
    //   / \
    //  1   *
    //     / \
    //    2   3
    //

    class Program
    {
        private static void Main(string[] args)
        {
            while(true)
            {
                Console.Write("> ");

                var line = Console.ReadLine();

                if(string.IsNullOrWhiteSpace(line))
                {
                    return;
                }

                var parser = new Parser(line);
                var syntaxTree = parser.Parse();

                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;

                PrettyPrint(syntaxTree.Root);

                Console.ForegroundColor = color;

                if (!syntaxTree.Diagnostics.Any())
                {
                    var e = new Evaluator(syntaxTree.Root);
                    var result = e.Evaluate();
                    Console.WriteLine(result);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    foreach (var entry in syntaxTree.Diagnostics)
                    {
                        Console.WriteLine(entry);
                    }

                    Console.ForegroundColor = color;
                }


                //var lexer = new Lexer(line);
                //while (true)
                //{
                //    var token = lexer.NextToken();

                //    if (token.Kind == SyntaxKind.EofToken) break;

                //    Console.Write($"{token.Kind}: '{token.Text}'");

                //    if(token.Value != null) Console.Write($" ({token.Value})");

                //    Console.WriteLine();
                //}
            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└── " : "├── ";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);

            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += isLast ? "    " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                PrettyPrint(child, indent, child == lastChild);
            }
        }
    }

    enum SyntaxKind
    {
        NumberToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParensToken,
        CloseParensToken,
        BadToken,
        EofToken,
        NumberExpression,
        BinaryExpression,
        ParenthesizedExpression
    }

    class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }


        public int Position { get; }

        public string Text { get; }

        public object Value { get; }
        public override SyntaxKind Kind { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }

    class Lexer
    {
        private readonly string _text;
        private int _position;

        public Lexer(string text)
        {
            _text = text;
            Diagnostics = new List<string>();
        }

        public List<string> Diagnostics { get; }

        private char Current
        {
            get
            {
                if (_position >= _text.Length) return '\0';

                return _text[_position];
            }
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken NextToken()
        {
            // <numbers>
            // + - * / ( )
            // <whitespace>

            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EofToken, _position, "\0", null);
            }

            if (char.IsDigit(Current))
            {
                var start = _position;

                while (char.IsDigit(Current)) Next();

                var length = _position - start;
                var text = _text.Substring(start, length);

                if (!int.TryParse(text, out var value))
                {
                    Diagnostics.Add($"The number {_text} cannot be represented an Int32");
                }

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if (char.IsWhiteSpace(Current))
            {
                var start = _position;

                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);

                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParensToken, _position++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParensToken, _position++, ")", null);
                default:
                    Diagnostics.Add($"ERROR: Bad character input: '{Current}'");
                    return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
            }
        }
    }

    abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }

    abstract class ExpressionSyntax : SyntaxNode
    {

    }

    sealed class NumberExpressionSyntax : ExpressionSyntax
    {
        public NumberExpressionSyntax(SyntaxToken numberToken)
        {
            NumberToken = numberToken;
        }

        public override SyntaxKind Kind => SyntaxKind.NumberExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return NumberToken;
        }

        public SyntaxToken NumberToken { get; }
    }

    sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }

        public ExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Right { get; }
    }

    sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
    {
        public ParenthesizedExpressionSyntax(SyntaxToken openParensToken, ExpressionSyntax expression, SyntaxToken closeParensToken)
        {
            OpenParensToken = openParensToken;
            Expression = expression;
            CloseParensToken = closeParensToken;
        }

        public SyntaxToken OpenParensToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseParensToken { get; }

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParensToken;
            yield return Expression;
            yield return CloseParensToken;
        }
    }

    sealed class SyntaxTree
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
    }

    class Parser
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
                token = lexer.NextToken();

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

        private SyntaxToken Match(SyntaxKind kind)
        {
            if (Current.Kind == kind)
            {
                return NextToken();
            }

            Diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseTerm();
        }

        public SyntaxTree Parse()
        {
            var expression = ParseTerm();
            var eofToken = Match(SyntaxKind.EofToken);

            return new SyntaxTree(Diagnostics, expression, eofToken);
        }

        private ExpressionSyntax ParseTerm()
        {
            var left = ParseFactor();

            while (Current.Kind == SyntaxKind.PlusToken ||
                   Current.Kind == SyntaxKind.MinusToken ||
                   Current.Kind == SyntaxKind.StarToken ||
                   Current.Kind == SyntaxKind.SlashToken)
            {
                var operatorToken = NextToken();
                var right = ParseFactor();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParseFactor()
        {
            var left = ParsePrimaryExpression();

            while (Current.Kind == SyntaxKind.StarToken ||
                   Current.Kind == SyntaxKind.SlashToken)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.Kind == SyntaxKind.OpenParensToken)
            {
                var left = NextToken();
                var expression = ParseExpression();
                var right = Match(SyntaxKind.CloseParensToken);

                return new ParenthesizedExpressionSyntax(left, expression, right);
            }

            var numberToken = Match(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }
    }

    class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            _root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            // BinaryExpression
            // NumberExpression

            switch (node)
            {
                case NumberExpressionSyntax n:
                    return (int) n.NumberToken.Value;
                case BinaryExpressionSyntax b:
                {
                    var left = EvaluateExpression(b.Left);
                    var right = EvaluateExpression(b.Right);

                    switch (b.OperatorToken.Kind)
                    {
                        case SyntaxKind.PlusToken:
                            return left + right;
                        case SyntaxKind.MinusToken:
                            return left - right;
                        case SyntaxKind.StarToken:
                            return left * right;
                        case SyntaxKind.SlashToken:
                            return left / right;
                        default:
                            throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}");
                    }
                }
                case ParenthesizedExpressionSyntax p:
                    return EvaluateExpression(p.Expression);
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }
    }
}
