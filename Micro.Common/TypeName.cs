using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Micro.Common
{
    public class TypeName
    {
        public string Namespace { get; }

        public string Name { get; }

        public TypeName[] Generics { get; }

        public TypeName(string @namespace, string name, params TypeName[] generics)
        {
            Namespace = @namespace;
            Name = name;
            Generics = generics;
        }

        public override string ToString()
        {
            if (Namespace == null)
                return Name;

            var str = $"{Namespace}.{Name}";
            if (Generics.Length > 0)
            {
                var generics = Generics.Select(x => x.ToString());
                str += $"<{string.Join(", ", $"{generics}")}>";
            }

            return str;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TypeName other))
                return false;

            return other.Namespace == Namespace && other.Name == Name;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static TypeName FromSymbol(ITypeSymbol symbol)
        {
            if (symbol == null)
                return new TypeName(null, "void");

            var displayString = symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            IEnumerable<string> parts = displayString.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.First() == "global")
                parts = parts.Skip(1);

            var partCount = parts.Count();
            if (partCount == 0)
                throw new Exception($@"Symbol display string ""{displayString}"" is empty");

            var name = parts.Last();

            var ns = partCount > 1 
                ? string.Join(".", parts.Take(partCount - 1)) 
                : null;

            TypeName[] generics;
            if (symbol is INamedTypeSymbol namedSymbol)
            {
                generics = namedSymbol.TypeParameters
                    .Select(x => FromSymbol(x.DeclaringType))
                    .ToArray();
            }
            else
            {
                generics = new TypeName[0];
            }

            return new TypeName(ns, name, generics);
        }

        public static bool operator ==(TypeName left, TypeName right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(TypeName left, TypeName right)
        {
            return !(left == right);
        }
    }
}
