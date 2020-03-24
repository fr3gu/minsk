using System;

namespace Minsk.Core.CodeAnalysis
{
    public sealed class VariableSymbol
    {
        public string Name { get; }
        public Type Type { get; }

        public VariableSymbol(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}