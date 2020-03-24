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
                        var operand = EvaluateExpression(u.Operand);

                        switch (u.OperatorKind)
                        {
                            case BoundUnaryOperatorKind.Identity:
                                return (int)operand;
                            case BoundUnaryOperatorKind.Negation:
                                return -(int)operand;
                            case BoundUnaryOperatorKind.LogicalNegation:
                                return !(bool) operand;
                            default:
                                throw new Exception($"Unexpected unary operator {u.OperatorKind}");
                        }
                    }
                case BoundBinaryExpression b:
                    {
                        var left = EvaluateExpression(b.Left);
                        var right = EvaluateExpression(b.Right);

                        switch (b.OperatorKind)
                        {
                            case BoundBinaryOperatorKind.Addition:
                                return (int)left + (int)right;
                            case BoundBinaryOperatorKind.Subtraction:
                                return (int)left - (int)right;
                            case BoundBinaryOperatorKind.Multiplication:
                                return (int)left * (int)right;
                            case BoundBinaryOperatorKind.Division:
                                return (int)left / (int)right;
                            case BoundBinaryOperatorKind.LogicalAndAlso:
                                return (bool)left && (bool)right;
                            case BoundBinaryOperatorKind.LogicalOrElse:
                                return (bool)left || (bool)right;
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