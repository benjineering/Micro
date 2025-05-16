using Micro.Server.Generators;
using Micro.Server.SyntaxParsers;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Micro.Server
{
    [Generator(LanguageNames.CSharp)]
    public class SourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            var generator = new LambdaGenerator();

            var requestHandlers = initContext.SyntaxProvider.ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: "Micro.RequestHandlerAttribute", // TODO: const
                predicate: (x, _) => true,
                transform: (x, _) => ClassParser.Parse(x)
            ).Collect();

            initContext.RegisterSourceOutput(requestHandlers, (context, parseResult) =>
            {
                var diagnostics = parseResult
                    .SelectMany(x => x.Diagnostics)
                    .Where(x => x != null)
                    .ToArray();

                foreach (var diagnostic in diagnostics)
                    context.ReportDiagnostic(diagnostic);

                var classes = parseResult.Select(x => x.Class);

                try
                {
                    generator.Generate(context, classes);
                }
                catch (Exception ex)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                        new DiagnosticDescriptor("UM1000", "Error generating code", ex.Message, "Micro", DiagnosticSeverity.Error, true),
                        null));
                }
            });
        }
    }
}
