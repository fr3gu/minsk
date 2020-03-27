using System.Collections.Generic;
using Minsk.Core.CodeAnalysis.Text;

namespace Minsk.Core.CodeAnalysis.Syntax
{
    internal class Lexer
    {
        private readonly SourceText _text;
        private int _position;
        private int _start;
        private SyntaxKind _kind;
        private object _value;

        public Lexer(SourceText text)
        {
            _text = text;
            Diagnostics = new DiagnosticsBag();
        }

        public DiagnosticsBag Diagnostics { get; }

        private char Current => Peek(0);

        private char Peek(int offset)
        {
            var index = _position + offset;
            return index >= _text.Length ? '\0' : _text[index];
        }

        public SyntaxToken Lex()
        {
            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;


            switch (Current)
            {
                case '\0':
                    _kind = SyntaxKind.EofToken;
                    break;
                case '+':
                    _kind = SyntaxKind.PlusToken;
                    _position++;
                    break;
                case '-':
                    _kind = SyntaxKind.MinusToken;
                    _position++;
                    break;
                case '*':
                    _kind = SyntaxKind.StarToken;
                    _position++;
                    break;
                case '/':
                    _kind = SyntaxKind.SlashToken;
                    _position++;
                    break;
                case '(':
                    _kind = SyntaxKind.OpenParensToken;
                    _position++;
                    break;
                case ')':
                    _kind = SyntaxKind.CloseParensToken;
                    _position++;
                    break;
                case '{':
                    _kind = SyntaxKind.OpenBraceToken;
                    _position++;
                    break;
                case '}':
                    _kind = SyntaxKind.CloseBraceToken;
                    _position++;
                    break;
                case '&':
                    _position++;
                    if (Current == '&')
                    {
                        _kind = SyntaxKind.AmpersandAmpersandToken;
                        _position++;
                    }

                    break;
                case '|':
                    _position++;
                    if (Current == '|')
                    {
                        _kind = SyntaxKind.PipePipeToken;
                        _position++;
                    }

                    break;
                case '=':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.EqualsToken;
                    }
                    else
                    {
                        _kind = SyntaxKind.EqualsEqualsToken;
                        _position++;
                    }

                    break;
                case '!':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.BangToken;
                    }
                    else
                    {
                        _kind = SyntaxKind.BangEqualsToken;
                        _position++;
                    }
                    break;
                case '<':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.LessThanToken;
                    }
                    else
                    {
                        _kind = SyntaxKind.LessThanOrEqualToken;
                        _position++;
                    }
                    break;
                case '>':
                    _position++;
                    if (Current != '=')
                    {
                        _kind = SyntaxKind.GreaterThanToken;
                    }
                    else
                    {
                        _kind = SyntaxKind.GreaterThanOrEqualToken;
                        _position++;
                    }
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    ReadNumberToken();
                    break;
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    ReadWhiteSpace();
                    break;
                default:
                    if (char.IsLetter(Current))
                    {
                        ReadIdentifierOrKeyword();
                    }
                    else if (char.IsWhiteSpace(Current))
                    {
                        ReadWhiteSpace();
                    }
                    else
                    {
                        Diagnostics.ReportBadCharacter(_position, Current);
                        _position++;
                    }
                    break;
            }

            var text2 = _kind.GetText();
            if (text2 == null)
            {
                var length = _position - _start;
                text2 = _text.ToString(_start, length);
            }
            return new SyntaxToken(_kind, _start, text2, _value);
        }

        private void ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Current))
            {
                _position++;
            }

            _kind = SyntaxKind.WhitespaceToken;
        }

        private void ReadNumberToken()
        {
            while (char.IsDigit(Current))
            {
                _position++;
            }

            var length = _position - _start;
            var text = _text.ToString(_start, length);

            if (!int.TryParse(text, out var value))
            {
                Diagnostics.ReportInvalidNumber(new TextSpan(_start, length), text, typeof(int));
            }

            _kind = SyntaxKind.NumberToken;
            _value = value;
        }

        private void ReadIdentifierOrKeyword()
        {
            while (char.IsLetter(Current))
            {
                _position++;
            }

            var lenght = _position - _start;
            var text = _text.ToString(_start, lenght);
            _kind = SyntaxFacts.GetKeywordKind(text);
        }
    }
}