using System;
using System.Collections.Generic;
using System.Text;
using Minsk.Core.CodeAnalysis.Text;
using NUnit.Framework;

namespace Minsk.Test.CodeAnalysis.Text.TextSpanTests
{
    public class FromBounds_Should
    {
        [TestCase(0, 8, 8)]
        [TestCase(1, 8, 7)]
        [TestCase(3, 3, 0)]
        public void CreateTextSpan_GivenStartAndLength(int start, int end, int expectedLength)
        {
            var span = TextSpan.FromBounds(start, end);

            Assert.That(span.Length, Is.EqualTo(expectedLength));
        }
    }
}
