using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minsk.Core.CodeAnalysis.Syntax;
using NUnit.Framework;

namespace Minsk.Test.CodeAnalysis.Syntax.LexterTests
{
    public class Lex_Should
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

        private static IEnumerable<object[]> GetTokenData()
        {
            foreach (var (kind, text) in GetTokens().Concat(GetSeparators()))
            {
                yield return new object[] {kind, text};
            }
        }

        private static IEnumerable<object[]> GetTokenPairsData()
        {
            foreach (var (t1Kind, t1Text, t2Kind, t2Text) in GetTokenPairs())
            {
                yield return new object[] { t1Kind, t1Text, t2Kind, t2Text };
            }
        }

        private static IEnumerable<object[]> GetTokenPairsWithSeparatorsData()
        {
            foreach (var (t1Kind, t1Text, sKind, sText, t2Kind, t2Text) in GetTokenPairsWithSeparator())
            {
                yield return new object[] { t1Kind, t1Text, sKind, sText, t2Kind, t2Text };
            }
        }

        private static bool RequiresSeparator(SyntaxKind t1Kind, SyntaxKind t2Kind)
        {
            var t1IsKeyword = t1Kind.ToString().EndsWith("Keyword");
            var t2IsKeyword = t2Kind.ToString().EndsWith("Keyword");

            if (t1Kind == SyntaxKind.IdentifierToken && t2Kind == SyntaxKind.IdentifierToken)
            {
                return true;
            }

            if (t1IsKeyword && t2IsKeyword)
            {
                return true;
            }

            if (t1IsKeyword && t2Kind == SyntaxKind.IdentifierToken)
            {
                return true;
            }

            if (t1Kind == SyntaxKind.IdentifierToken && t2IsKeyword)
            {
                return true;
            }

            if (t1Kind == SyntaxKind.NumberToken && t2Kind == SyntaxKind.NumberToken)
            {
                return true;
            }

            var unabletopairwithBang = new List<SyntaxKind>
            {
                SyntaxKind.EqualsEqualsToken,
                SyntaxKind.EqualsToken
            };

            if (t1Kind == SyntaxKind.BangToken && unabletopairwithBang.Contains(t2Kind))
            {
                return true;
            }

            var unabletopairwithEquals = new List<SyntaxKind>
            {
                SyntaxKind.EqualsEqualsToken,
                SyntaxKind.EqualsToken
            };

            if (t1Kind == SyntaxKind.EqualsToken && unabletopairwithEquals.Contains(t2Kind))
            {
                return true;
            }

            if (t1Kind == SyntaxKind.LessThanToken && unabletopairwithEquals.Contains(t2Kind))
            {
                return true;
            }

            if (t1Kind == SyntaxKind.GreaterThanToken && unabletopairwithEquals.Contains(t2Kind))
            {
                return true;
            }

            var unabletopairwithAmpersand = new List<SyntaxKind>
            {
                SyntaxKind.AmpersandAmpersandToken,
                SyntaxKind.AmpersandToken
            };

            if (t1Kind == SyntaxKind.AmpersandToken && unabletopairwithAmpersand.Contains(t2Kind))
            {
                return true;
            }

            var unabletopairwithPipe = new List<SyntaxKind>
            {
                SyntaxKind.PipePipeToken,
                SyntaxKind.PipeToken
            };

            if (t1Kind == SyntaxKind.PipeToken && unabletopairwithPipe.Contains(t2Kind))
            {
                return true;
            }

            return false;
        }

        private static IEnumerable<(SyntaxKind Kind, string Text)> GetTokens()
        {
            var fixedTokens = Enum.GetValues(typeof(SyntaxKind))
                .Cast<SyntaxKind>()
                .Select(k => (kind: k, text: k.GetText()))
                .Where(k => k.text != null);

            var dynamicTokens = new[]
            {
                (SyntaxKind.NumberToken, "9"),
                (SyntaxKind.NumberToken, "123"),
                (SyntaxKind.IdentifierToken, "a"),
                (SyntaxKind.IdentifierToken, "abd")
            };

            return fixedTokens.Concat(dynamicTokens);
        }

        private static IEnumerable<(SyntaxKind Kind, string Text)> GetSeparators()
        {
            return new[]
            {
                (SyntaxKind.WhitespaceToken, " "),
                (SyntaxKind.WhitespaceToken, "  "),
                (SyntaxKind.WhitespaceToken, "\r"),
                (SyntaxKind.WhitespaceToken, "\n"),
                (SyntaxKind.WhitespaceToken, "\r\n")
            };
        }

        private static IEnumerable<(SyntaxKind T1Kind, string T1Text, SyntaxKind T2Kind, string T2Text)> GetTokenPairs()
        {
            foreach (var (t1Kind, t1Text) in GetTokens())
            {
                foreach (var (t2Kind, t2Text) in GetTokens())
                {
                    if (!RequiresSeparator(t1Kind, t2Kind))
                    {
                        yield return (t1Kind, t1Text, t2Kind, t2Text);
                    }

                }
            }
        }

        private static IEnumerable<(SyntaxKind T1Kind, string T1Text, SyntaxKind SeparatorKind, string SeparatorText, SyntaxKind T2Kind, string T2Text)> GetTokenPairsWithSeparator()
        {
            foreach (var (t1Kind, t1Text) in GetTokens())
            {
                foreach (var (t2Kind, t2Text) in GetTokens())
                {
                    if (!RequiresSeparator(t1Kind, t2Kind)) continue;

                    foreach (var (sKind, sText) in GetSeparators())
                    {
                        yield return (t1Kind, t1Text, sKind, sText, t2Kind, t2Text);
                    }

                }
            }
        }
    }
}
