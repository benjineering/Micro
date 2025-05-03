using Micro.CodeGen.Models;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Micro.CodeGen.SyntaxParser
{
    static class ClassParser
    {
        public static ClassParserResult Parse(GeneratorAttributeSyntaxContext context)
        {
            var klass = context.TargetSymbol.ContainingType;
            var name = klass.Name;
            var ns = GetNamespace(klass);

            var methodResults = klass.GetMembers()
                .OfType<IMethodSymbol>()
                .Select(x => ParseMethod(context, x))
                .ToArray();

            var diagnostics = methodResults
                .Select(x => x.Diagnostic)
                .Where(x => x != null)
                .ToArray();

            return diagnostics.Length > 0
                ? new ClassParserResult { Diagnostics = diagnostics } // TODO: handle warnings
                : new ClassParserResult
                {
                    Class = new Class
                    {
                        Name = name,
                        Namespace = ns,
                        Methods = methodResults.Select(x => x.Method).ToArray(),
                    },
                };
        }

        private static MethodParserResult ParseMethod(GeneratorAttributeSyntaxContext context, IMethodSymbol method)
        {
            var returnTypeNs = GetNamespace(method.ReturnType);

            if (returnTypeNs != nameof(Micro) || method.ReturnType.Name != nameof(Response))
            {
                return new MethodParserResult
                {
                    Diagnostic = Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "UM100",
                            title: "Micro request handlers must return Micro.Result or System.Task<Micro.Result>",
                            messageFormat: "",
                            category: nameof(Micro),
                            defaultSeverity: DiagnosticSeverity.Error,
                            isEnabledByDefault: true), // TODO: const error codes & messages
                        method.Locations.FirstOrDefault()),
                };
            }

            // TODO: handle duplicate method names (overloading)

            return new MethodParserResult
            {
                Method = new Method
                {
                    Name = method.Name,
                    ReturnType = method.ReturnType,
                    Parameters = method.Parameters.ToArray(),
                },
            };
        }

        private static string GetNamespace(ISymbol symbol)
        {
            return symbol.ContainingNamespace?.ToDisplayString(
                SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted));
        }
    }
}
