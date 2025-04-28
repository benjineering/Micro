using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace Micro.CodeGen
{
    [Generator(LanguageNames.CSharp)]
    public class SourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            //var textFiles = initContext.AdditionalTextsProvider
            //    .Where(file => file.Path.EndsWith("+server.svelte"));

            var requestHandlers = initContext.SyntaxProvider.CreateSyntaxProvider(
                predicate: (x, _) => NodeIsRequestHandlerClass(x),
                transform: (x, _) => GetRequestHandlerClassMethods(x)
            )
            .Where(x => x != null);

            initContext.RegisterSourceOutput(requestHandlers, (context, symbol) =>
            {
                // TODO
                context.AddSource("Test.g.cs", $@"namespace Plops
{{
    public class McGee
    {{
        public const string MrMeow = ""meowers"";
    }}
}}");
            });
        }

        private static bool NodeIsRequestHandlerClass(SyntaxNode node)
        {
            if (!(node is ClassDeclarationSyntax klass) || klass.AttributeLists.Count == 0)
                return false;

            foreach (var attrList in klass.AttributeLists)
            {
                foreach (var attr in attrList.Attributes)
                {
                    // TODO: fix this hot mess (name is different between test and dev)
                    var attrName = attr.Name.ToFullString();
                    if (attrName == "RequestHandler" || attrName == "Micro.Requests.RequestHandlerAttribute")
                        return true;
                }
            }

            return false;
        }

        private static (string ClassName, IMethodSymbol[] Methods)? GetRequestHandlerClassMethods(GeneratorSyntaxContext context)
        {
            if (!(context.Node is ClassDeclarationSyntax klass))
                return null;

            foreach (var child in klass.ChildNodesAndTokens())
            {
                //child.
            }

            return null;
        }
    }
}
