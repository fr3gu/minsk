﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using Minsk.Core.CodeAnalysis.Binding;
using Minsk.Core.CodeAnalysis.Syntax;

namespace Minsk.Core.CodeAnalysis
{
    public sealed class Compilation
    {
        private BoundGlobalScope _globalScope;

        public Compilation Previous { get; }
        public SyntaxTree SyntaxTree { get; }

        public Compilation(SyntaxTree syntaxTree) : this(null, syntaxTree)
        {
        }

        private Compilation(Compilation previous, SyntaxTree syntaxTree)
        {
            //_globalScope = null;
            Previous = previous;
            SyntaxTree = syntaxTree;
        }

        internal BoundGlobalScope GlobalScope
        {
            get
            {
                if (_globalScope != null) return _globalScope;

                var globalScope = Binder.BindGlobalScope(Previous?.GlobalScope, SyntaxTree.Root);
                Interlocked.CompareExchange(ref _globalScope, globalScope, null);

                return _globalScope;
            }
        }

        public Compilation ContinueWith(SyntaxTree syntaxTree)
        {
            return new Compilation(this, syntaxTree);
        }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables)
        {
            var diagnostics = SyntaxTree.Diagnostics.Concat(GlobalScope.Diagnostics).ToImmutableArray();

            if (diagnostics.Any())
            {
                return new EvaluationResult(diagnostics, null);
            }

            var evaluator = new Evaluator(GlobalScope.Statement, variables);
            var value = evaluator.Evaluate();

            return new EvaluationResult(ImmutableArray<Diagnostic>.Empty, value);
        }

        public void EmitTree(TextWriter writer)
        {
            _globalScope.Statement.WriteTo(writer);
        }
    }
}