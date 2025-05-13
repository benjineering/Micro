using Micro.Common;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Micro.Server.SyntaxParsers
{
    static class ClassParser
    {
        public static ClassParserResult Parse(GeneratorAttributeSyntaxContext context)
        {
            if (!(context.TargetSymbol is INamedTypeSymbol props))
                return new ClassParserResult
                {
                    Diagnostics = MicroDiagnostics.CreateArray(MicroDiagnosticType.NotAClass, context.TargetSymbol.Locations),
                };

            // TODO: read serialized classes

            var name = TypeName.FromSymbol(props);

            var methodResults = props.GetMembers()
                .OfType<IMethodSymbol>()
                .Where(x => x.MethodKind != MethodKind.Constructor)
                .Select(ParseMethod)
                .ToArray();

            var diagnostics = methodResults
                .Select(x => x.Diagnostic)
                .Where(x => x != null)
                .ToArray();

            var methods = methodResults
                .Where(x => x != null)
                .Select(x => x.Method)
                .ToArray();

            return  new ClassParserResult
            {
                Diagnostics = diagnostics,
                Class = new Class(name, methods),
            };
        }

        private static MethodParserResult ParseMethod(IMethodSymbol method)
        {
            var parameters = method.Parameters
                .Select(x =>
                {
                    var typeName = TypeName.FromSymbol(x.Type);
                    return new Parameter(x.Name, typeName);
                })
                .ToArray();

            var returnType = TypeName.FromSymbol(method.ReturnType);

            return new MethodParserResult
            {
                Method = new Method(method.Name, parameters, returnType),
            };
        }
    }
}
