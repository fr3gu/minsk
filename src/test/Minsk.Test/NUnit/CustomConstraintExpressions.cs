using Minsk.Test.NUnit.Constraints;
using NUnit.Framework.Constraints;

namespace Minsk.Test.NUnit
{
    internal static class CustomConstraintExpressions
    {
        public static DiagnosticsConstraint Diagnostics(this ConstraintExpression expression, object expected)
        {
            var constraint = new DiagnosticsConstraint(expected);
            expression.Append(constraint);

            return constraint;
        }
    }
}