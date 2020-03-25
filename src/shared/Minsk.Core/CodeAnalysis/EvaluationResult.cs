using System.Collections.Immutable;

namespace Minsk.Core.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(ImmutableArray<Diagnostic> diagnostics, object value)
        {
            Value = value;
            Diagnostics = diagnostics;
        }

        public ImmutableArray<Diagnostic> Diagnostics { get; }
        public object Value { get; }
    }
}