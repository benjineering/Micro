using Micro.Server.Models;
using Microsoft.CodeAnalysis;
using System.Linq;

namespace Micro.Server.SyntaxParsers
{
    static class ClassParser
    {
        private static readonly TypeName[] _validMethodReturnTypes = new TypeName[]
        {
            new TypeName("Micro", "Response"),
            new TypeName("System.Threading.Tasks", "Task", new TypeName("Micro", "Response")),
        };

        public static ClassParserResult Parse(GeneratorAttributeSyntaxContext context)
        {
            if (!(context.TargetSymbol is INamedTypeSymbol klass))
                return new ClassParserResult
                {
                    Diagnostics = MicroDiagnostics.CreateArray(MicroDiagnosticType.NotAClass, context.TargetSymbol.Locations),
                };

            var name = TypeName.FromSymbol(klass);

            var methodResults = klass.GetMembers()
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

            return new ClassParserResult
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
            if (!_validMethodReturnTypes.Contains(returnType))
                return new MethodParserResult
                {
                    Diagnostic = MicroDiagnostics.Create(MicroDiagnosticType.WrongReturnType, method.Locations),
                };

            // TODO: handle duplicate method names (overloading)

            return new MethodParserResult
            {
                Method = new Method(method.Name, parameters, returnType),
            };
        }
    }
}
