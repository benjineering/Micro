using Micro.CodeGen.Models;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Micro.CodeGen.SyntaxParser
{
    static class ClassParser
    {
        public static Klass Parse(GeneratorAttributeSyntaxContext context)
        {
            var klass = context.TargetSymbol.ContainingType;

            var name = klass.Name;
            var ns = klass.ContainingNamespace?.ToDisplayString(
                SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted));

            var methods = klass.GetMembers()
                .OfType<IMethodSymbol>()
                .Select(x =>new Method
                {
                    Name = x.Name,
                    ReturnType = x.ReturnType,
                    Parameters = x.Parameters.ToArray()
                })
                .Where(x => x != null)
                .ToArray();

            return new Klass
            {
                Name = name,
                Namespace = ns,
            };
        }
    }
}
