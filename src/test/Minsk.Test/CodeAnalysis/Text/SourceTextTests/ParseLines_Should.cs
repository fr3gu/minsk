using System;
using System.Collections.Generic;
using System.Text;
using Minsk.Core.CodeAnalysis.Text;
using NUnit.Framework;

namespace Minsk.Test.CodeAnalysis.Text.SourceTextTests
{
    public class ParseLines_Should
    {
        [TestCase(".", 1)]
        [TestCase(".\r\n\r\n", 3)]
        public void IncludeLastLine(string text, int expectedLineCount)
        {
            var sut = SourceText.From(text);

            var lines = sut.Lines;

            Assert.That(lines, Has.Exactly(expectedLineCount).Items);
        }
    }
}
