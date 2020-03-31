using System;

namespace Minsk.Core.CodeAnalysis
{
    public sealed class VariableSymbol
    {
        public string Name { get; }
        public bool IsReadonly { get; }
        public Type Type { get; }

        public VariableSymbol(string name, bool isReadonly, Type type)
        {
            Name = name;
            IsReadonly = isReadonly;
            Type = type;
        }

        public override string ToString() => Name;
    }
}