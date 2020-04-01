using System;
using System.Collections.Generic;
using System.Linq;
using Minsk.Core.CodeAnalysis.Syntax;
using Minsk.Core.CodeAnalysis.Text;
using NUnit.Framework;

namespace Minsk.Test.CodeAnalysis.Syntax.LexterTests
{
    public class Lex_Should : LexerTestsBase
    {
        [TestCaseSource(nameof(GetTokenData))]
        public void LexTokensAllPossibleTokens(SyntaxKind kind, string text)
        {
            var tokenKinds = Enum.GetValues(typeof(SyntaxKind))
                .Cast<SyntaxKind>()
                .Where(k => k.ToString().EndsWith("Keyword") ||
                            k.ToString().EndsWith("Token"))
                .ToList();

            var testedTokenKinds = GetTokens().Concat(GetSeparators()).Select(t => t.Kind);

            var untestedTokenKinds = new SortedSet<SyntaxKind>(tokenKinds);
            untestedTokenKinds.Remove(SyntaxKind.BadToken);
            untestedTokenKinds.Remove(SyntaxKind.EofToken);
            untestedTokenKinds.ExceptWith(testedTokenKinds);

            Assert.That(untestedTokenKinds, Is.Empty);
        }

        [TestCaseSource(nameof(GetTokenData))]
        public void LexTokens_Given_SyntaxKind(SyntaxKind kind, string text)
        {
            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.That(tokens, Has.One.Items);
            Assert.That(tokens[0].Kind, Is.EqualTo(kind));
            Assert.That(tokens[0].Text, Is.EqualTo(text));
        }

        [TestCaseSource(nameof(GetTokenPairsData))]
        public void LexTokenPairs_Given_SyntaxKind(SyntaxKind t1Kind, string t1Text, SyntaxKind t2Kind, string t2Text)
        {
            var text = t1Text + t2Text;

            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.That(tokens, Has.Exactly(2).Items);
            Assert.That(tokens[0].Kind, Is.EqualTo(t1Kind));
            Assert.That(tokens[0].Text, Is.EqualTo(t1Text));
            Assert.That(tokens[1].Kind, Is.EqualTo(t2Kind));
            Assert.That(tokens[1].Text, Is.EqualTo(t2Text));
        }

        [TestCaseSource(nameof(GetTokenPairsWithSeparatorsData))]
        public void LexTokenPairsWithSeparators_Given_SyntaxKind(SyntaxKind t1Kind, string t1Text, SyntaxKind sKind, string sText, SyntaxKind t2Kind, string t2Text)
        {
            var text = t1Text + sText + t2Text;

            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.That(tokens, Has.Exactly(3).Items);
            Assert.That(tokens[0].Kind, Is.EqualTo(t1Kind));
            Assert.That(tokens[0].Text, Is.EqualTo(t1Text));
            Assert.That(tokens[1].Kind, Is.EqualTo(sKind));
            Assert.That(tokens[1].Text, Is.EqualTo(sText));
            Assert.That(tokens[2].Kind, Is.EqualTo(t2Kind));
            Assert.That(tokens[2].Text, Is.EqualTo(t2Text));
        }

        [Test]
        public void Handle_UnterminatedString()
        {
            const string text = "\"text";

            var tokens = SyntaxTree.ParseTokens(text, out var diagnostics).ToArray();

            Assert.That(tokens, Has.One.Items);
            Assert.That(tokens[0].Kind, Is.EqualTo(SyntaxKind.StringToken));
            Assert.That(tokens[0].Text, Is.EqualTo(text));

            Assert.That(diagnostics, Has.One.Items);
            Assert.That(diagnostics[0].Span, Is.EqualTo(new TextSpan(0, 1)));
            Assert.That(diagnostics[0].Message, Is.EqualTo("Unterminated string literal."));
        }
    }
}
