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
            var expression = SyntaxTree.Parse(text);

            if (op1Precedence >= op2Precedence)
            {
                //      op2
                //     /   \
                //   op1    c
                //  /   \
                // a     b
                using (var asserter = new AssertingEnumerator(expression.Root))
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

                using (var asserter = new AssertingEnumerator(expression.Root))
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
    }
}