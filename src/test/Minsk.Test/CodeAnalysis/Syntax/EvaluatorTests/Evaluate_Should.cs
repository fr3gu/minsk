using System;
using System.Collections.Generic;
using System.Linq;
using Minsk.Core.CodeAnalysis;
using Minsk.Core.CodeAnalysis.Syntax;
using NUnit.Framework;

namespace Minsk.Test.CodeAnalysis.Syntax.EvaluatorTests
{
    public class Evaluate_Should
    {
        [TestCase("1", 1)]
        [TestCase("+1", 1)]
        [TestCase("-1", -1)]
        [TestCase("1 + 2", 3)]
        [TestCase("1+2", 3)]
        [TestCase("1 + 2 + 3", 6)]
        [TestCase("1 + 2 * 3", 7)]
        [TestCase("3 * 3", 9)]
        [TestCase("12 - 9", 3)]
        [TestCase("12 / 3", 4)]
        [TestCase("(10)", 10)]
        [TestCase("(1 + 2) * 8", 24)]
        [TestCase("true", true)]
        [TestCase("false", false)]
        [TestCase("!true", false)]
        [TestCase("!false", true)]
        [TestCase("1 == 1", true)]
        [TestCase("1 == 2", false)]
        [TestCase("1 != 2", true)]
        [TestCase("1 != 1", false)]
        [TestCase("true && true", true)]
        [TestCase("true && false", false)]
        [TestCase("false || true", true)]
        [TestCase("false || false", false)]
        [TestCase(" 1 < 3", true)]
        [TestCase(" 1 > 3", false)]
        [TestCase(" 3 > 1", true)]
        [TestCase(" 3 < 1", false)]
        [TestCase(" 3 >= 1", true)]
        [TestCase(" 3 >= 3", true)]
        [TestCase(" 3 >= 5", false)]
        [TestCase(" 1 <= 2", true)]
        [TestCase(" 2 <= 2", true)]
        [TestCase(" 5 <= 2", false)]
        [TestCase("{ var a = 0 (a = 10) * a }", 100)]
        [TestCase("{ var a = 0 if a == 0 a = 10 }", 10)]
        [TestCase("{ var a = 0 if a == 5 a = 10 }", 0)]
        [TestCase("{ var a = 0 if a == 5 a = 10 else a = 20 }", 20)]
        [TestCase("{ var a = 0 if a == 0 a = 10 else a = 20 }", 10)]
        [TestCase("{ var a = 0 while (a < 9) a = a + 1 }", 9)]
        [TestCase("{ var i = 10 var result = 0 while (i > 0) { result = result + i i = i - 1 } result }", 55)]
        [TestCase("{ var result = 0 for i = 5 to 15 { result = result + 1 } result }", 11)]
        public void AccuratelyEvaluateExpressions(string text, object expected)
        {
            var expression = SyntaxTree.Parse(text);
            var compilation = new Compilation(expression);
            var variables = new Dictionary<VariableSymbol, object>();

            var evaluationResult = compilation.Evaluate(variables);

            Assert.That(evaluationResult.Diagnostics, Is.Empty);

            Assert.That(evaluationResult.Value, Is.EqualTo(expected));
        }

        [Test]
        public void NotEndUpInInfiniteLoop_GivenIncompleteStatement()
        {
            var text = @"
                {
                    [)][]
            ";

            var diagnostics = @"
                ERROR: Unexpected token <CloseParensToken>, expected <IdentifierToken>
                ERROR: Unexpected token <EofToken>, expected <CloseBraceToken>
            ";
            AssertHasDiagnostics(text, diagnostics);
        }

        [Test]
        public void ReportRedeclaration_Given_AssignmentExpression()
        {
            var text = @"
            {
                var x = 100
                var y = 10
                {
                    var x = 10
                }
                var [x] = 5
            }";

            var expectedDiagnostic = "Variable 'x' is already declared";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        [Test]
        public void ReportUndefinedVariable_GivenUndefinedVariable()
        {
            var text = @"[a] + 10";

            var expectedDiagnostic = "Variable 'a' doesn't exist";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        [Test]
        public void ReportCannotAssign_GivenAssignmentExpressionWithReadonlyVariable()
        {
            var text = @"
            {
                let y = 10
                y [=] 0
            }
            ";

            var expectedDiagnostic = "Variable 'y' is readonly and cannot be assigned to";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        [Test]
        public void ReportCannotConvert_GivenAssignmentExpressionWithIntVariableAndBoolValue()
        {
            var text = @"
            {
                var b = 10
                [b] = false
            }
            ";

            var expectedDiagnostic = "Cannot convert from <System.Boolean> to <System.Int32>";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        [Test]
        public void ReportCannotConvert_Given_IfStatement()
        {
            var text = @"
            {
                var x = 0
                if [10]
                    x = 10
            }
            ";

            var expectedDiagnostic = "Cannot convert from <System.Int32> to <System.Boolean>";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        [Test]
        public void ReportCannotConvert_Given_WhileStatement()
        {
            var text = @"
            {
                var x = 0
                var result = 0
                while [10]
                    result = result + x
            }
            ";

            var expectedDiagnostic = "Cannot convert from <System.Int32> to <System.Boolean>";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        [Test]
        public void ReportCannotConvert_ForLowerBound_GivenForStatement()
        {
            var text = @"
            {
                var x = 0
                var result = 0
                for x = [false] to 10
                    result = result + x
            }
            ";

            var expectedDiagnostic = "Cannot convert from <System.Boolean> to <System.Int32>";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        [Test]
        public void ReportCannotConvert_ForUpperBound_GivenForStatement()
        {
            var text = @"
            {
                var x = 0
                var result = 0
                for x = 10 to [false]
                    result = result + x
            }
            ";

            var expectedDiagnostic = "Cannot convert from <System.Boolean> to <System.Int32>";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        [Test]
        public void ReportUndefinedUnaryOperator_GivenUnaryExpression()
        {
            var text = @"[+]true";

            var expectedDiagnostic = "Unary operator '+' is not defined for type <System.Boolean>";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        [Test]
        public void ReportUndefinedBinaryOperator_GivenBinaryExpression()
        {
            var text = @"1 [&&] true";

            var expectedDiagnostic = "Binary operator '&&' is not defined for type <System.Int32> and <System.Boolean>";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        [Test]
        public void Report_UnexpectedToken()
        {
            var text = @"1 [(] + 4";

            var expectedDiagnostic = "ERROR: Unexpected token <OpenParensToken>, expected <EofToken>";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        [Test]
        public void ReportUnexpectedToken_GivenEmptyExpression()
        {
            var text = @"[]";

            var expectedDiagnostic = "ERROR: Unexpected token <EofToken>, expected <IdentifierToken>";

            AssertHasDiagnostics(text, expectedDiagnostic);
        }

        private void AssertHasDiagnostics(string text, string diagnosticText)
        {
            var annotatedText = AnnotatedText.Parse(text);
            var syntaxTree = SyntaxTree.Parse(annotatedText.Text);
            var compilation = new Compilation(syntaxTree);
            var result = compilation.Evaluate(new Dictionary<VariableSymbol, object>());
            var diagnostics = AnnotatedText.UnindentLines(diagnosticText);

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
                throw new Exception($"Inconsisten number of '[]' and expected diagnostics. Found {diagnostics.Length} diagnostics, expected {annotationsCount} based on number of '[]'");
            }

            for (var i = 0; i < diagnostics.Length; i++)
            {
                var expectedDiagnostic = diagnostics[i];
                var actualDiagnostic = result.Diagnostics[i].Message;

                Assert.That(actualDiagnostic, Is.EqualTo(expectedDiagnostic));

                var expectedSpan = annotatedText.Spans[i];
                var actualSpan = result.Diagnostics[i].Span;

                Assert.That(expectedSpan, Is.EqualTo(actualSpan));
            }
        }
    }
}