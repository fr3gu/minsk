using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using Minsk.Core.CodeAnalysis.Text;

namespace Minsk.Test
{
    internal sealed class AnnotatedText
    {
        public string Text { get; }
        public ImmutableArray<TextSpan> Spans { get; }

        public AnnotatedText(string text, ImmutableArray<TextSpan> spans)
        {
            Text = text;
            Spans = spans;
        }

        public static AnnotatedText Parse(string text)
        {
            text = Unindent(text);

            var textBuilder = new StringBuilder();
            var spanBuilder = ImmutableArray.CreateBuilder<TextSpan>();
            var starts = new Stack<int>();

            var position = 0;

            foreach (var c in text)
            {
                if (c == '[')
                {
                    starts.Push(position);
                }
                else if (c == ']')
                {
                    if(starts.Count == 0)
                    {
                        throw new ArgumentException("Too many '['", nameof(text));
                    }

                    var start = starts.Pop();
                    var end = position;
                    var span = TextSpan.FromBounds(start, end);
                    spanBuilder.Add(span);
                }
                else
                {
                    position++;
                    textBuilder.Append(c);
                }

            }

            if (starts.Count != 0)
            {
                throw new ArgumentException("Missing ']'", nameof(text));
            }
            //var text = @"
            //    {
            //        var x = 10
            //        let x = 2
            //    }
            //";

            return new AnnotatedText(textBuilder.ToString(), spanBuilder.ToImmutable());
        }

        private static string Unindent(string text)
        {
            var lines = UnindentLines(text);

            return string.Join(Environment.NewLine, lines);
        }

        public static string[] UnindentLines(string text)
        {
            var lines = new List<string>();

            using (var stringReader = new StringReader(text))
            {
                string line;
                while ((line = stringReader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            var minIndentation = int.MaxValue;
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];

                if (line.Trim().Length == 0)
                {
                    lines[i] = string.Empty;
                    continue;
                }

                var indentation = line.Length - line.Trim().Length;
                minIndentation = Math.Min(indentation, minIndentation);
            }

            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].Length == 0) continue;

                lines[i] = lines[i].Substring(minIndentation);
            }

            while (lines.Count > 0 && lines[0].Length == 0)
                lines.RemoveAt(0);

            while (lines.Count > 0 && lines[^1].Length == 0)
                lines.RemoveAt(lines.Count - 1);

            return lines.ToArray();
        }
    }
}