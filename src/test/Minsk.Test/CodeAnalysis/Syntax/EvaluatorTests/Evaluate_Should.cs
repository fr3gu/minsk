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
        [TestCase("(a = 10) * a", 100)]
        public void AccuratelyEvaluateExpressions(string text, object expected)
        {
            var expression = SyntaxTree.Parse(text);
            var compilation = new Compilation(expression);
            var variables = new Dictionary<VariableSymbol, object>();

            var evaluationResult = compilation.Evaluate(variables);

            Assert.That(evaluationResult.Diagnostics, Is.Empty);

            Assert.That(evaluationResult.Value, Is.EqualTo(expected));
        }
    }
}