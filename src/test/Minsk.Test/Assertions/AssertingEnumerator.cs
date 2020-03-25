using System;
using System.Collections.Generic;
using System.Linq;
using Minsk.Core.CodeAnalysis.Syntax;
using NUnit.Framework;

namespace Minsk.Test.Assertions
{
    internal sealed class AssertingEnumerator : IDisposable
    {
        private readonly IEnumerator<SyntaxNode> _enumerator;
        private bool _hasErrors;

        public AssertingEnumerator(SyntaxNode node)
        {
            _enumerator = Flatten(node).GetEnumerator();
            _hasErrors = false;
        }

        public void Dispose()
        {
            if (!_hasErrors)
                Assert.That(_enumerator.MoveNext(), Is.False);
            _enumerator.Dispose();
        }

        private bool MarkFailed()
        {
            _hasErrors = true;
            return false;
        }

        private static IEnumerable<SyntaxNode> Flatten(SyntaxNode node)
        {
            var stack = new Stack<SyntaxNode>();

            stack.Push(node);

            while (stack.Count > 0)
            {
                var n = stack.Pop();
                yield return n;

                foreach (var child in n.GetChildren().Reverse())
                {
                    stack.Push(child);
                }
            }
        }

        public void AssertToken(SyntaxKind kind, string text)
        {
            try
            {
                Assert.That(_enumerator.MoveNext(), Is.True);
                var current = _enumerator.Current;
                Assert.That(current, Is.Not.Null);
                Assert.That(current.Kind, Is.EqualTo(kind));
                Assert.That(current, Is.TypeOf(typeof(SyntaxToken)));
                var syntaxToken = current as SyntaxToken;
                Assert.That(syntaxToken, Is.Not.Null);
                Assert.That(syntaxToken.Text, Is.EqualTo(text));
            }
            catch when (MarkFailed())
            {
                throw;
            }

        }

        public void AssertNode(SyntaxKind kind)
        {
            try
            {
                Assert.That(_enumerator.MoveNext(), Is.True);
                var current = _enumerator.Current;
                Assert.That(current, Is.Not.Null);
                Assert.That(current.Kind, Is.EqualTo(kind));
                Assert.That(current, Is.Not.TypeOf(typeof(SyntaxToken)));
            }
            catch when (MarkFailed())
            {
                throw;
            }
        }
    }
}