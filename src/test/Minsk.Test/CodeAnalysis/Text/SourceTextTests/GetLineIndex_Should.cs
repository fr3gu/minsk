using System;
using System.Collections.Generic;
using System.Text;
using Minsk.Core.CodeAnalysis.Text;
using NUnit.Framework;

namespace Minsk.Test.CodeAnalysis.Text.SourceTextTests
{
    public class GetLineIndex_Should
    {
        [TestCase("aa\r\nbb\r\ncc\r\ndd\r\nee", 8, 2)]
        [TestCase("aa\r\nbb\r\ncc\r\ndd\r\nee", 6, 1)]
        [TestCase("en katt skickade ett paket\r\ntill en morbror som bodde i\r\nUkraina", 57, 2)]
        [TestCase("en katt skickade ett paket\r\ntill en morbror som bodde i\r\nUkraina", 55, 1)]
        public void IncludeLastLine(string text, int position, int expectedLineIndex)
        {
            var sut = SourceText.From(text);

            var lineIndex = sut.GetLineIndex(position);

            Console.WriteLine(sut.ToString());

            Assert.That(lineIndex, Is.EqualTo(expectedLineIndex));
        }
    }
}
