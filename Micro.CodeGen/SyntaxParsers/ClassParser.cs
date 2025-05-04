using Micro.CodeGen.Models;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Micro.CodeGen.SyntaxParsers
{
    static class ClassParser
    {
        private static readonly string[] _validMethodReturnTypes = new string[]
        {
            "global::Micro.Response",
            "global::System.Threading.Tasks.Task<global::Micro.Response>",
        };

        public static ClassParserResult Parse(GeneratorAttributeSyntaxContext context)
        {
            if (!(context.TargetSymbol is INamedTypeSymbol klass))
                return new ClassParserResult
                {
                    Diagnostics = new Diagnostic[]
                    {
                        Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "UM200",
                            title: "Not a class",
                            messageFormat: "[RequestHandler] should only be applied to a class",
                            category: nameof(Micro),
                            defaultSeverity: DiagnosticSeverity.Error,
                            isEnabledByDefault: true), // TODO: const error codes & messages
                        context.TargetSymbol?.Locations.FirstOrDefault()),
                    }
                };

            var name = klass.Name;
            var ns = GetNamespace(klass);

            var methodResults = klass.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(x => x.MethodKind != MethodKind.Constructor)
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
            var returnType = method.ReturnType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            if (!_validMethodReturnTypes.Contains(returnType))
            {
                return new MethodParserResult
                {
                    Diagnostic = Diagnostic.Create(
                        new DiagnosticDescriptor(
                            "UM201",
                            title: "Wrong return type",
                            messageFormat: $"Micro request handlers return one of ({string.Join(", ", _validMethodReturnTypes)}) - is {returnType}",
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
