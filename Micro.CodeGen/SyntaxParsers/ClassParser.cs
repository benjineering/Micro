using Micro.CodeGen.Models;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Micro.CodeGen.SyntaxParsers
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
                .Select(ParseMethod)
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

        private static MethodParserResult ParseMethod(IMethodSymbol method)
        {
            var returnTypeIsValid = IsValidReturnType(method.ReturnType);
            if (!returnTypeIsValid)
            {
                return new MethodParserResult
                {
                    Diagnostic = Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "UM200",
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

        private static bool IsValidReturnType(ITypeSymbol type)
        {
            if (GetNamespace(type) == nameof(Micro) && type.Name == nameof(Response))
                return true;

            if (GetNamespace(type) != "System.Threading.Tasks" || type.Name != nameof(System.Threading.Tasks.Task))
                return false;

            if (!(type is INamedTypeSymbol namedType) || namedType.IsGenericType || namedType.TypeParameters.Length != 1)
                return false;

            var paramType = namedType.TypeParameters.First();
            return GetNamespace(paramType) == nameof(Micro) && paramType.Name == nameof(Response);
        }
    }
}
