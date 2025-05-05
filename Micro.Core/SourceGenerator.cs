using Micro.Common;
using Micro.Core.Generators;
using Micro.Core.SyntaxParsers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Micro.Core
{
    [Generator(LanguageNames.CSharp)]
    public class SourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            var requestHandlers = initContext.SyntaxProvider.ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: "Micro.RequestHandlerAttribute", // TODO: const
                predicate: (x, _) => x is ClassDeclarationSyntax,
                transform: (x, _) => ClassParser.Parse(x)
            )
                .Collect();

            initContext.RegisterSourceOutput(requestHandlers, (context, parseResult) =>
            {
                var diagnostics = parseResult
                    .SelectMany(x => x.Diagnostics)
                    .Where(x => x != null)
                    .ToArray();

                if (diagnostics.Length > 0)
                {
                    foreach (var diagnostic in diagnostics)
                        context.ReportDiagnostic(diagnostic);

                    return; // TODO: handle warnings
                }

                var classes = parseResult.Select(x => x.Class);

                try
                {
                    ClassGenerator.Generate(context, classes);
                }
                catch (Exception ex)
                {
                    context.ReportDiagnostic(MicroDiagnostics.Create(MicroDiagnosticType.ClassGeneratorError, messageOverride: ex.Message));
                }

                try
                {
                    JsonContextGenerator.Generate(context, classes);
                }
                catch (Exception ex)
                {
                    context.ReportDiagnostic(MicroDiagnostics.Create(MicroDiagnosticType.JsonContextGeneratorError, messageOverride: ex.Message));
                }
            });
        }
    }
}
