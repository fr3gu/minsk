using System;
using mc.CodeAnalysis.Binding;

namespace mc.CodeAnalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;

        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private object EvaluateExpression(BoundExpression node)
        {

            switch (node)
            {
                case BoundLiteralExpression n:
                    return n.Value;
                case BoundUnaryExpression u:
                {
                    var operand = (int)EvaluateExpression(u.Operand);

                    switch (u.OperatorKind)
                    {
                        case BoundUnaryOperatorKind.Identity:
                            return operand;
                        case BoundUnaryOperatorKind.Negation:
                            return -operand;
                        default:
                            throw new Exception($"Unexpected unary operator {u.OperatorKind}");
                    }
                }
                case BoundBinaryExpression b:
                {
                    var left = (int)EvaluateExpression(b.Left);
                    var right = (int)EvaluateExpression(b.Right);

                    switch (b.OperatorKind)
                    {
                        case BoundBinaryOperatorKind.Addition:
                            return left + right;
                        case BoundBinaryOperatorKind.Subtraction:
                            return left - right;
                        case BoundBinaryOperatorKind.Multiplication:
                            return left * right;
                        case BoundBinaryOperatorKind.Division:
                            return left / right;
                        default:
                            throw new Exception($"Unexpected binary operator {b.OperatorKind}");
                    }
                }
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }
    }
}