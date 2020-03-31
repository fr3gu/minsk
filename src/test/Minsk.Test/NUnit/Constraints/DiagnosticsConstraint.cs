using System;
using System.Collections.Generic;
using Minsk.Core.CodeAnalysis;
using Minsk.Core.CodeAnalysis.Syntax;
using NUnit.Framework.Constraints;

namespace Minsk.Test.NUnit.Constraints
{
    internal class DiagnosticsConstraint : Constraint
    {
        public object Expected { get; }

        public DiagnosticsConstraint(object expected)
        {
            Expected = expected;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var theText = actual as string;
            var annotatedText = AnnotatedText.Parse(theText);
            var syntaxTree = SyntaxTree.Parse(annotatedText.Text);
            var compilation = new Compilation(syntaxTree);
            var result = compilation.Evaluate(new Dictionary<VariableSymbol, object>());

            var diagnostics = AnnotatedText.UnindentLines((string)Expected);

            var annotationsCount = annotatedText.Spans.Length;
            var actualDiagsCount = result.Diagnostics.Length;
            if (annotationsCount < actualDiagsCount)
            {
                throw new Exception($"Too few markers '[]'. Found {annotationsCount}, expected {actualDiagsCount}");
            }

            if (annotationsCount > actualDiagsCount)
            {
                throw new Exception($"Too many markers '[]'. Found {annotationsCount}, expected {actualDiagsCount}");
            }

            if (annotationsCount != diagnostics.Length)
            {
                throw new Exception($"Inconsistent number of '[]' and expected diagnostics. Found {diagnostics.Length} diagnostics, expected {annotationsCount} based on number of '[]'");
            }

            for (var i = 0; i < diagnostics.Length; i++)
            {
                var expectedDiagnostic = diagnostics[i];
                var actualDiagnostic = result.Diagnostics[i].Message;

                if (!actualDiagnostic.Equals(expectedDiagnostic))
                {
                    Description = $"{expectedDiagnostic}";
                    return new ConstraintResult(this, actualDiagnostic, false);
                }

                var expectedSpan = annotatedText.Spans[i];
                var actualSpan = result.Diagnostics[i].Span;

                if (!expectedSpan.Equals(actualSpan))
                {
                    Description = $"{expectedSpan}";
                    return new ConstraintResult(this, actualSpan, false);
                }
            }

            return new ConstraintResult(this, actual, true);
        }
    }
}