using System.Collections.Generic;
using System.Linq;

namespace Minsk.Core.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(IEnumerable<Diagnostic> diagnostics, object value)
        {
            Value = value;
            Diagnostics = diagnostics.ToArray();
        }

        public IReadOnlyList<Diagnostic> Diagnostics { get; }
        public object Value { get; }
    }
}