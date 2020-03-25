using System;
using System.Collections.Generic;
using System.Linq;
using Minsk.Core.CodeAnalysis.Syntax;
using NUnit.Framework;

namespace Minsk.Test.CodeAnalysis.Syntax.SyntaxFactsTests
{
    public class GetText_Should
    {
        [TestCaseSource(nameof(GetSyntaxKindData))]
        public void Rountrtrip_GivenSyntaxKind(SyntaxKind kind)
        {
            var text = kind.GetText();

            if (text == null)
            {
                return;
            }

            var tokens = SyntaxTree.ParseTokens(text).ToArray();

            Assert.That(tokens, Has.One.Items);
            Assert.That(tokens[0].Kind, Is.EqualTo(kind));
            Assert.That(tokens[0].Text, Is.EqualTo(text));
        }

        private static IEnumerable<object[]> GetSyntaxKindData()
        {
            var kinds = (SyntaxKind[])Enum.GetValues(typeof(SyntaxKind));
            foreach (var syntaxKind in kinds)
            {
                yield return new object[] { syntaxKind };
            }
        }
    }
}