using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Minsk.Core.CodeAnalysis;
using Minsk.Core.CodeAnalysis.Syntax;
using Minsk.Core.CodeAnalysis.Text;

namespace Minsk.ConsoleApp
{

    internal class Program
    {
        private static void Main()
        {
            var showTree = false;
            var variables = new Dictionary<VariableSymbol, object>();
            var textBuilder = new StringBuilder();
            var keepGoing = true;

            while(keepGoing)
            {
                Console.ForegroundColor = ConsoleColor.Green;

                if(textBuilder.Length == 0)
                {
                    Console.Write("» ");
                }
                else
                {
                    Console.Write("· ");
                }

                Console.ResetColor();

                var input = Console.ReadLine();
                var isBlank = string.IsNullOrWhiteSpace(input);

                if (textBuilder.Length == 0)
                {
                    if(isBlank)
                    {
                        return;
                    }
                    switch (input)
                    {
                        case "#showTree":
                            showTree = !showTree;
                            Console.WriteLine(showTree ? "Showing parse tree." : "Not showing parse tree.");
                            continue;
                        case "#cls":
                            Console.Clear();
                            continue;
                        case "#quit":
                            keepGoing = false;
                            break;
                    }
                }

                textBuilder.AppendLine(input);
                var text = textBuilder.ToString();

                var syntaxTree = SyntaxTree.Parse(text);

                if (!isBlank && syntaxTree.Diagnostics.Any())
                {
                    continue;
                }

                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate(variables);

                var diagnostics = result.Diagnostics.ToArray();

                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;

                if(showTree) syntaxTree.Root.WriteTo(Console.Out);

                Console.ForegroundColor = color;

                if (!diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(result.Value);
                    Console.ResetColor();
                }
                else
                {
                    foreach (var diagnostic in diagnostics)
                    {
                        var lineIndex = syntaxTree.Text.GetLineIndex(diagnostic.Span.Start);
                        var line = syntaxTree.Text.Lines[lineIndex];
                        var lineNumber = lineIndex + 1;
                        var character = diagnostic.Span.Start - line.Start + 1;

                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"({lineNumber}, {character}): ");
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();

                        var prefixSpan = TextSpan.FromBounds(line.Start, diagnostic.Span.Start);
                        var suffixSpan = TextSpan.FromBounds(diagnostic.Span.End, line.End);

                        var prefix = syntaxTree.Text.ToString(prefixSpan);
                        var error = syntaxTree.Text.ToString(diagnostic.Span);
                        var suffix = syntaxTree.Text.ToString(suffixSpan);

                        Console.Write("    ");
                        Console.Write(prefix);

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(error);
                        Console.ResetColor();

                        Console.Write(suffix);
                        Console.WriteLine();
                    }

                    Console.WriteLine();
                }

                textBuilder.Clear();
            }
        }
    }
}
