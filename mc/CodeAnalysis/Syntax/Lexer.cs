using System.Collections.Generic;

namespace mc.CodeAnalysis.Syntax
{
    internal class Lexer
    {
        private readonly string _text;
        private int _position;

        public Lexer(string text)
        {
            _text = text;
            Diagnostics = new List<string>();
        }

        public List<string> Diagnostics { get; }

        private char Current => Peek(0);
        private char LookAhead => Peek(1);

        private char Peek(int offset)
        {
            var index = _position + offset;
            return index >= _text.Length ? '\0' : _text[index];
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken Lex()
        {
            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EofToken, _position, "\0", null);
            }

            if (char.IsDigit(Current))
            {
                var start = _position;

                while (char.IsDigit(Current))
                    Next();

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

            if (char.IsLetter(Current))
            {
                var start = _position;

                while (char.IsLetter(Current))
                    Next();

                var lenght = _position - start;
                var text = _text.Substring(start, lenght);
                var kind = SyntaxFacts.GetKeywordKind(text);

                return new SyntaxToken(kind, start, text, null);
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
                case '&':
                    if (LookAhead == '&')
                    {
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, _position += 2, "&&", null);
                    }
                    break;
                case '|':
                    if (LookAhead == '|')
                    {
                        return new SyntaxToken(SyntaxKind.PipePipeToken, _position += 2, "||", null);
                    }
                    break;
                case '=':
                    if (LookAhead == '=')
                    {
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, _position += 2, "==", null);
                    }
                    break;
                case '!':
                    return LookAhead == '=' ?
                        new SyntaxToken(SyntaxKind.BangEqualsToken, _position += 2, "!=", null) :
                        new SyntaxToken(SyntaxKind.BangToken, _position++, "!", null);
            }

            Diagnostics.Add($"ERROR: Bad character input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}