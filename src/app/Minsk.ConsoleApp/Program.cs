using System;
using System.Collections.Generic;
using System.Linq;
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

            while(true)
            {
                Console.Write("> ");

                var line = Console.ReadLine();

                if(string.IsNullOrWhiteSpace(line))
                {
                    return;
                }

                if (line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse tree." : "Not showing parse tree.");
                    continue;
                }
                if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }
                if (line == "#quit")
                {
                    break;
                }

                var syntaxTree = SyntaxTree.Parse(line);
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate(variables);

                var diagnostics = result.Diagnostics.ToArray();

                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;

                if(showTree) syntaxTree.Root.WriteTo(Console.Out);

                Console.ForegroundColor = color;

                if (!diagnostics.Any())
                {
                    Console.WriteLine(result.Value);
                }
                else
                {
                    var text = syntaxTree.Text;

                    foreach (var entry in diagnostics)
                    {
                        var lineIndex = text.GetLineIndex(entry.Span.Start);
                        var lineNumber = lineIndex + 1;
                        var character = entry.Span.Start - text.Lines[lineIndex].Start + 1;

                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"({lineNumber}, {character}): ");
                        Console.WriteLine(entry);
                        Console.ResetColor();

                        var prefix = line.Substring(0, entry.Span.Start);
                        var error = line.Substring(entry.Span.Start, entry.Span.Length);
                        var suffix = line.Substring(entry.Span.End);

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
            }
        }
    }
}
