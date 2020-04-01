﻿using System;

namespace Minsk.Core.CodeAnalysis.Symbols
{
    public sealed class VariableSymbol : Symbol
    {
        internal VariableSymbol(string name, bool isReadonly, Type type) : base(name)
        {
            IsReadonly = isReadonly;
            Type = type;
        }

        public override SymbolKind Kind => SymbolKind.Variable;
        public bool IsReadonly { get; }
        public Type Type { get; }
    }
}