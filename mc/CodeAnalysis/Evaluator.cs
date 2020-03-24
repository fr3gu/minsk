using System;

namespace mc.CodeAnalysis
{
    internal class Evaluator
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