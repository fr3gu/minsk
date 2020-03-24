using System.Collections.Generic;
using System.Linq;

namespace Minsk.Core.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(IEnumerable<string> diagnostics, object value)
        {
            Value = value;
            Diagnostics = diagnostics.ToArray();
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public object Value { get; }
    }
}