﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace Minsk.Core.CodeAnalysis.Binding
{
    [ExcludeFromCodeCoverage]
    internal abstract class BoundNode
    {
        public abstract BoundNodeKind Kind { get;}

        public IEnumerable<BoundNode> GetChildren()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (typeof(BoundNode).IsAssignableFrom(property.PropertyType))
                {
                    var child = (BoundNode)property.GetValue(this);
                    if (child != null)
                        yield return child;
                }
                else if (typeof(IEnumerable<BoundNode>).IsAssignableFrom(property.PropertyType))
                {
                    var children = (IEnumerable<BoundNode>)property.GetValue(this);
                    foreach (var child in children)
                    {
                        if (child != null)
                            yield return child;
                    }
                }
            }
        }

        public void WriteTo(TextWriter writer)
        {
            PrettyPrint(writer, this);
        }

        public override string ToString()
        {
            using var writer = new StringWriter();

            WriteTo(writer);

            return writer.ToString();
        }

        private static ConsoleColor GetColor(BoundNode node)
        {
            switch (node)
            {
                case BoundExpression _:
                    return ConsoleColor.Blue;
                case BoundStatement _:
                    return ConsoleColor.Cyan;
                default:
                    return ConsoleColor.DarkYellow;
            }
        }

        private static void PrettyPrint(TextWriter writer, BoundNode node, string indent = "", bool isLast = true)
        {
            var isToConsole = writer == Console.Out;

            var marker = isLast ? "└── " : "├── ";

            if (isToConsole)
                Console.ForegroundColor = ConsoleColor.DarkGray;

            writer.Write(indent);
            writer.Write(marker);

            if (isToConsole)
                Console.ForegroundColor = GetColor(node);

            var text = GetText(node);
            writer.Write(text);

            var isFirstProp = true;

            foreach (var prop in node.GetProperties())
            {
                if (isFirstProp)
                {
                    isFirstProp = false;
                    writer.Write(" ");
                }
                else
                {
                    if (isToConsole)
                        Console.ForegroundColor = ConsoleColor.DarkGray;

                    writer.Write(", ");
                }

                if (isToConsole)
                    Console.ForegroundColor = ConsoleColor.Yellow;

                writer.Write(prop.Name);

                if (isToConsole)
                    Console.ForegroundColor = ConsoleColor.DarkGray;

                writer.Write(" = ");

                if (isToConsole)
                    Console.ForegroundColor = ConsoleColor.DarkYellow;

                writer.Write(prop.Value);
            }

            if (isToConsole)
                Console.ResetColor();

            writer.WriteLine();

            indent += isLast ? "    " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
            {
                PrettyPrint(writer, child, indent, child == lastChild);
            }
        }

        private static string GetText(BoundNode node)
        {
            switch (node)
            {
                case BoundBinaryExpression b:
                    return $"{b.Op.Kind}Expression";
                case BoundUnaryExpression u:
                    return $"{u.Op.Kind}Expression";
                default:
                    return node.Kind.ToString();
            }
        }

        private IEnumerable<(string Name, object Value)> GetProperties()
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (property.Name == nameof(Kind) || property.Name == nameof(BoundBinaryExpression.Op))
                {
                    continue;
                }

                if (typeof(BoundNode).IsAssignableFrom(property.PropertyType) || typeof(IEnumerable<BoundNode>).IsAssignableFrom(property.PropertyType))
                {
                    continue;
                }

                var value = property.GetValue(this);
                if (value != null)
                    yield return (property.Name, value);
            }
        }
    }
}