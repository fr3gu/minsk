using System.Collections.Generic;
using Minsk.Core.CodeAnalysis.Syntax;
using Minsk.Test.Assertions;
using NUnit.Framework;

namespace Minsk.Test.CodeAnalysis.Syntax.ParserTests
{
    public class Parse_Should
    {
        [TestCaseSource(nameof(GetBinaryOperatorsData))]
        public void HonorPrecedences_GivenBinaryExpression(SyntaxKind op1, SyntaxKind op2)
        {
            var op1Precedence = op1.GetBinaryOperatorPrecedence();
            var op2Precedence = op2.GetBinaryOperatorPrecedence();
            var op1Text = op1.GetText();
            var op2Text = op2.GetText();
            var text = $"a {op1Text} b {op2Text} c";
            var expressionSyntax = ParseExpression(text);

            if (op1Precedence >= op2Precedence)
            {
                //      op2
                //     /   \
                //   op1    c
                //  /   \
                // a     b
                using (var asserter = new AssertingEnumerator(expressionSyntax))
                {
                    asserter.AssertNode(SyntaxKind.BinaryExpression);
                    asserter.AssertNode(SyntaxKind.BinaryExpression);
                    asserter.AssertNode(SyntaxKind.NameExpression);
                    asserter.AssertToken(SyntaxKind.IdentifierToken, "a");
                    asserter.AssertToken(op1, op1Text);
                    asserter.AssertNode(SyntaxKind.NameExpression);
                    asserter.AssertToken(SyntaxKind.IdentifierToken, "b");
                    asserter.AssertToken(op2, op2Text);
                    asserter.AssertNode(SyntaxKind.NameExpression);
                    asserter.AssertToken(SyntaxKind.IdentifierToken, "c");
                }
            }
            else
            {
                //   op1
                //  /   \
                // a    op2
                //     /   \
                //    b     c

                using (var asserter = new AssertingEnumerator(expressionSyntax))
                {
                    asserter.AssertNode(SyntaxKind.BinaryExpression);
                    asserter.AssertNode(SyntaxKind.NameExpression);
                    asserter.AssertToken(SyntaxKind.IdentifierToken, "a");
                    asserter.AssertToken(op1, op1Text);
                    asserter.AssertNode(SyntaxKind.BinaryExpression);
                    asserter.AssertNode(SyntaxKind.NameExpression);
                    asserter.AssertToken(SyntaxKind.IdentifierToken, "b");
                    asserter.AssertToken(op2, op2Text);
                    asserter.AssertNode(SyntaxKind.NameExpression);
                    asserter.AssertToken(SyntaxKind.IdentifierToken, "c");
                }
            }
        }

        private static ExpressionSyntax ParseExpression(string text)
        {
            var syntaxTree = SyntaxTree.Parse(text);
            var root = syntaxTree.Root;
            var statement = root.Statement;
            Assert.That(statement, Is.TypeOf(typeof(ExpressionStatementSyntax)));
            return ((ExpressionStatementSyntax)statement).Expression;
        }

        [TestCaseSource(nameof(GetUnaryOperatorsData))]
        public void HonorPrecedences_GivenUnaryExpression(SyntaxKind unaaryKind, SyntaxKind binaryKind)
        {
            var unaryPrecedence = unaaryKind.GetUnaryOperatorPrecedence();
            var binaryPrecedence = binaryKind.GetBinaryOperatorPrecedence();
            var unaryText = unaaryKind.GetText();
            var binaryText = binaryKind.GetText();
            var text = $"{unaryText} a {binaryText} b";
            var expressionSyntax = ParseExpression(text);

            if (unaryPrecedence >= binaryPrecedence)
            {
                //   op2
                //  /   \
                // op1   b
                //  |
                //  a

                using (var asserter = new AssertingEnumerator(expressionSyntax))
                {
                    asserter.AssertNode(SyntaxKind.BinaryExpression);
                    asserter.AssertNode(SyntaxKind.UnaryExpression);
                    asserter.AssertToken(unaaryKind, unaryText);
                    asserter.AssertNode(SyntaxKind.NameExpression);
                    asserter.AssertToken(SyntaxKind.IdentifierToken, "a");
                    asserter.AssertToken(binaryKind, binaryText);
                    asserter.AssertNode(SyntaxKind.NameExpression);
                    asserter.AssertToken(SyntaxKind.IdentifierToken, "b");
                }
            }
            else
            {
                //   op2
                //    |
                //   op1
                //  /   \
                // a     b

                using (var asserter = new AssertingEnumerator(expressionSyntax))
                {
                    asserter.AssertNode(SyntaxKind.UnaryExpression);
                    asserter.AssertNode(SyntaxKind.BinaryExpression);
                    asserter.AssertToken(unaaryKind, unaryText);
                    asserter.AssertNode(SyntaxKind.NameExpression);
                    asserter.AssertToken(SyntaxKind.IdentifierToken, "a");
                    asserter.AssertToken(binaryKind, binaryText);
                    asserter.AssertNode(SyntaxKind.NameExpression);
                    asserter.AssertToken(SyntaxKind.IdentifierToken, "b");
                }
            }
        }

        private static IEnumerable<object[]> GetBinaryOperatorsData()
        {
            foreach (var op1 in SyntaxFacts.GetBinaryOperatorKinds())
            {
                foreach (var op2 in SyntaxFacts.GetBinaryOperatorKinds())
                {
                    yield return new object[] { op1, op2 };
                }
            }
        }

        private static IEnumerable<object[]> GetUnaryOperatorsData()
        {
            foreach (var op1 in SyntaxFacts.GetUnaryOperatorKinds())
            {
                foreach (var op2 in SyntaxFacts.GetBinaryOperatorKinds())
                {
                    yield return new object[] { op1, op2 };
                }
            }
        }
    }
}