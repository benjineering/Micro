using Micro.CodeGen.Config;
using Micro.CodeGen.Generators;
using Micro.CodeGen.SyntaxParser;
using Microsoft.CodeAnalysis;
using System;

namespace Micro.CodeGen
{
    [Generator(LanguageNames.CSharp)]
    public class SourceGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext initContext)
        {
            // TODO: pass config from project (we can't use system json, and i'm not sure how aot works with newtonsoft? might be fine...)
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

            //var textFiles = initContext.AdditionalTextsProvider
            //    .Where(file => file.Path.EndsWith("+server.svelte"));

            var requestHandlers = initContext.SyntaxProvider.CreateSyntaxProvider(
                predicate: (x, _) => ClassParser.NodeIsRequestHandlerClass(x),
                transform: (x, _) => ClassParser.Parse(x)
            )
            .Where(x => x != null)
            .Collect();

            initContext.RegisterSourceOutput(requestHandlers, (context, klass) =>
                generator.Generate(context, klass));
        }
    }
}
