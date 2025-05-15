using Micro.Config;
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
            // TODO: pass config from project (we can't use system json,
            // and i'm not sure how aot works with newtonsoft? might be fine...)
            // ...maybe editor config or sth?
            var config = new MicroConfig();
            IGenerator generator;
            switch (config.ServerGeneratorType)
            {
                case ServerGeneratorType.Http:
                    generator = new HttpGenerator();
                    break;
                case ServerGeneratorType.Lambda:
                    generator = new LambdaGenerator();
                    break;
                default:
                    throw new Exception($"Unsupported ServerGeneratorType {config.ServerGeneratorType}");
            }

            var requestHandlers = initContext.SyntaxProvider.ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: "Micro.CoreRequestHandlerAttribute", // TODO: const
                predicate: (x, _) => true,
                transform: (x, _) => ClassParser.Parse(x)
            );

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
