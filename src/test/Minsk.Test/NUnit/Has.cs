using Minsk.Test.NUnit.Constraints;

namespace Minsk.Test.NUnit
{
    internal class Has : global::NUnit.Framework.Has {
        public static DiagnosticsConstraint Diagnostics(object expected)
        {
            return new DiagnosticsConstraint(expected);
        }
    }
}