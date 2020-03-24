using System;
using System.Collections.Generic;
using System.Linq;
using Minsk.Core.CodeAnalysis;
using Minsk.Core.CodeAnalysis.Syntax;

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

                if(showTree) PrettyPrint(syntaxTree.Root);

                Console.ForegroundColor = color;

                if (!diagnostics.Any())
                {
                    Console.WriteLine(result.Value);
                }
                else
                {

                    foreach (var entry in diagnostics)
                    {
                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.Red;
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

        private static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└── " : "├── ";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);

            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += isLast ? "    " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                PrettyPrint(child, indent, child == lastChild);
            }
        }
    }
}
