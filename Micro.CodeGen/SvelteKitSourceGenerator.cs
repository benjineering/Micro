using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;

namespace Micro.CodeGen
{
    [Generator(LanguageNames.CSharp)]
    public class SvelteKitSourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            //var textFiles = initContext.AdditionalTextsProvider
            //    .Where(file => file.Path.EndsWith("+server.svelte"));

            var requestHandlers = initContext.SyntaxProvider.CreateSyntaxProvider(
                predicate: (s, _) => s is MethodDeclarationSyntax m && m.AttributeLists.Count > 0,
                transform: (ctx, _) => GetMethodIfHasTargetAttribute(ctx)
            )
            .Where(m => m != null);

            initContext.RegisterSourceOutput(requestHandlers, (context, symbol) =>
            {
                // TODO
                context.AddSource("Test.g.cs", $@"namespace Plops
{{
    const string MrMeow = ""meowers"";
}}");
            });
        }

        private static IMethodSymbol GetMethodIfHasTargetAttribute(GeneratorSyntaxContext context)
        {
            var methodSyntax = context.Node as MethodDeclarationSyntax;

            if (!(context.SemanticModel.GetDeclaredSymbol(methodSyntax) is IMethodSymbol symbol))
                return null;

            foreach (var attr in symbol.GetAttributes())
            {
                File.WriteAllText(
                    @"C:\Users\8enwi\source\repos\benjineering\micros\Micro\Micro.Console\GeneratedFiles\log.txt",
                    $"{attr.AttributeClass.ToDisplayString()}");

                var attrName = attr.AttributeClass.ToDisplayString(); // TODO: Check full namespace somehow
                if (attrName == "RequestHandler" || attrName == "Micro.Common.Requests.RequestHandlerAttribute")
                    return symbol;
            }

            return null;
        }
    }
}
