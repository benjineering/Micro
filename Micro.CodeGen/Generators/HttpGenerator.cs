using Micro.CodeGen.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Micro.CodeGen.Generators
{
    class HttpGenerator : IGenerator
    {
        public void Generate(SourceProductionContext context, IEnumerable<Class> klass)
        {
            // TODO:
            //   - generate app.MapMicroEndpoints();
            //   - generate TS https://chatgpt.com/c/68115978-a344-800d-83fc-ec969fcb0a24
            //      - models (create a common generator)
            //      - client

            var source = @"using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Micro.IoC
{
    public static class EndpointExtensions
    {
        public static void ConfigureMicroJsonContexts(this IServiceCollection services)
        {

        }

        public static void MapMicroEndpoints(this WebApplication app)
        {

        }
    }
}
";
            context.AddSource("MicroHttp.g.cs", source);
        }
    }
}

//builder.Services.ConfigureHttpJsonOptions(options =>
//{
//    foreach (var (context, i) in Micro.Generated.SerializerContexts.Select((x, i) => (x, i)))
//    {
//        options.SerializerOptions.TypeInfoResolverChain.Insert(i, context);
//    }
//});

//foreach (var endpointGroup in Micro.Generated.EndpointGroups)
//{
//    var group = app.MapGroup($"/{endpointGroup.Name}");

//    foreach (var endpoint in group.Endpoints)
//    {
//        group.MapGet($"/{endpoint.Name}", () => endpoint.Method);
//    }
//}
