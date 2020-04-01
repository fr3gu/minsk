using System;
using System.Collections;
using System.Collections.Generic;
using Minsk.Core.CodeAnalysis.Symbols;
using Minsk.Core.CodeAnalysis.Syntax;
using Minsk.Core.CodeAnalysis.Text;

namespace Minsk.Core.CodeAnalysis
{
    internal sealed class DiagnosticsBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Report(TextSpan span, string message)
        {
            _diagnostics.Add(new Diagnostic(span, message));
        }

        public void ReportInvalidNumber(TextSpan span, string text, TypeSymbol type)
        {
            Report(span, $"The number {text} isn't valid <{type}>");
        }

        public void ReportBadCharacter(int position, char current)
        {
            var message = $"Bad character input: '{current}'";
            Report(new TextSpan(position, 1), message);
        }

        public void ReportUnterminatedString(TextSpan span)
        {
            var message = $"Unterminated string literal.";
            Report(span, message);
        }

        public void AddRange(DiagnosticsBag diagnostics)
        {
            _diagnostics.AddRange(diagnostics);
        }

        public void ReportUnexpectedToken(TextSpan span, SyntaxKind currentKind, SyntaxKind expectedKind)
        {
            var message = $"ERROR: Unexpected token <{currentKind}>, expected <{expectedKind}>";
            Report(span, message);

        }

        public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, TypeSymbol operandType)
        {
            var message = $"Unary operator '{operatorText}' is not defined for type <{operandType}>";
            Report(span, message);
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string operatorText, TypeSymbol leftOperandType, TypeSymbol rightOperandType)
        {
            var message = $"Binary operator '{operatorText}' is not defined for type <{leftOperandType}> and <{rightOperandType}>";
            Report(span, message);
        }

        public void ReportUndefinedName(TextSpan span, string name)
        {
            var message = $"Variable '{name}' doesn't exist";
            Report(span, message);
        }

        public void ReportCannotConvert(TextSpan span, TypeSymbol fromType, TypeSymbol toType)
        {
            var message = $"Cannot convert from <{fromType}> to <{toType}>";
            Report(span, message);
        }

        public void ReportVariableAlreadyDeclared(TextSpan span, string name)
        {
            var message = $"Variable '{name}' is already declared";
            Report(span, message);
        }

        public void ReportCannotAssign(TextSpan span, string name)
        {
            var message = $"Variable '{name}' is readonly and cannot be assigned to";
            Report(span, message);
        }
    }
}