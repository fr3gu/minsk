using System;
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
                var result = compilation.Evaluate();

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
                    Console.ForegroundColor = ConsoleColor.Red;

                    foreach (var entry in diagnostics)
                    {
                        Console.WriteLine(entry);
                    }

                    Console.ForegroundColor = color;
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
