﻿using System;
using System.Collections;
using System.Collections.Generic;
using Minsk.Core.CodeAnalysis.Syntax;

namespace Minsk.Core.CodeAnalysis
{
    public sealed class Diagnostic
    {
        public Diagnostic(TextSpan span, string message)
        {
            Span = span;
            Message = message;
        }

        public TextSpan Span { get; }
        public string Message { get; }

        public override string ToString() => Message;
    }

    internal sealed class DiagnosticsBag : IEnumerable<Diagnostic>
    {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Report(TextSpan span, string message)
        {
            _diagnostics.Add(new Diagnostic(span, message));
        }

        public void ReportInvalidNumber(TextSpan span, string text, Type type)
        {
            Report(span, $"The number {text} isn't valid {type}");
        }

        public void ReportBadCharacter(int position, char current)
        {
            var message = $"Bad character input: '{current}'";
            Report(new TextSpan(position, 1), message);
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

        public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, Type operandType)
        {
            var message = $"Unary operator '{operatorText}' is not defined for type {operandType}";
            Report(span, message);
        }

        public void ReportUndefinedBinaryOperator(TextSpan span, string operatorText, Type leftOperandType, Type rightOperandType)
        {
            var message = $"Binary operator '{operatorText}' is not defined for type {leftOperandType} and {rightOperandType}";
            Report(span, message);
        }
    }
}