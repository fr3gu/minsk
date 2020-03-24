using System;
using System.Collections.Generic;
using Minsk.Core.CodeAnalysis.Binding;

namespace Minsk.Core.CodeAnalysis
{
    internal sealed class Evaluator
    {
        private readonly BoundExpression _root;
        private readonly Dictionary<string, object> _variables;

        public Evaluator(BoundExpression root, Dictionary<string, object> variables)
        {
            _root = root;
            _variables = variables;
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
                case BoundVariableExpression v:
                    return _variables[v.Name];
                case BoundAssignmentExpression a:
                    var value = EvaluateExpression(a.Expression);
                    _variables[a.Name] = value;
                    return value;
                case BoundUnaryExpression u:
                    {
                        var operand = EvaluateExpression(u.Operand);

                        switch (u.Op.Kind)
                        {
                            case BoundUnaryOperatorKind.Identity:
                                return (int)operand;
                            case BoundUnaryOperatorKind.Negation:
                                return -(int)operand;
                            case BoundUnaryOperatorKind.LogicalNegation:
                                return !(bool) operand;
                            default:
                                throw new Exception($"Unexpected unary operator {u.Op}");
                        }
                    }
                case BoundBinaryExpression b:
                    {
                        var left = EvaluateExpression(b.Left);
                        var right = EvaluateExpression(b.Right);

                        switch (b.Op.Kind)
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
                            case BoundBinaryOperatorKind.Equals:
                                return Equals(left, right);
                            case BoundBinaryOperatorKind.NotEquals:
                                return !Equals(left, right);
                            default:
                                throw new Exception($"Unexpected binary operator {b.Op}");
                        }
                    }
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }
    }
}