using System;
using System.Linq;
using Minsk.Core.CodeAnalysis.Syntax;

namespace Minsk.Core.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {
        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type Type { get; }

        public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type) :
            this(syntaxKind, kind, type, type)
        {
        }

        public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type, Type resultType) :
            this(syntaxKind, kind, type, type, resultType)
        {
        }

        public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            Type = resultType;
        }

        private static readonly BoundBinaryOperator[] _operators =
        {
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.SlashToken, BoundBinaryOperatorKind.Division, typeof(int)),

            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.Equals, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EqualsEqualsToken, BoundBinaryOperatorKind.NotEquals, typeof(int), typeof(bool)),

            new BoundBinaryOperator(SyntaxKind.AmpersandAmpersandToken, BoundBinaryOperatorKind.LogicalAndAlso, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.PipePipeToken, BoundBinaryOperatorKind.LogicalOrElse, typeof(bool)),
        };

        public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
        {
            return _operators.FirstOrDefault(op => op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType);
        }
    }
}
