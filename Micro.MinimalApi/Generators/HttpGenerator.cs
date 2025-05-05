using Micro.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Micro.MinimalApi.Generators
{
    class HttpGenerator : IGenerator
    {
        public void Generate(SourceProductionContext context, IEnumerable<Class> classes)
        {
            // TODO:
            //   - JsonContexts (1 per class)
            //   - generate TS https://chatgpt.com/c/68115978-a344-800d-83fc-ec969fcb0a24
            //      - models (create a common generator)
            //      - client

            var endpoints = classes
                .Select(klass =>
                {
                    string path;
                    string action;

                    if (klass.Namespace == null)
                    {
                        path = "/" + klass.Namespace.Replace('.', '/') + klass.Name;
                        action = klass.Namespace + '.' + klass.Name;
                    }
                    else
                    {
                        path = "/" + klass.Name;
                        action = klass.Name;
                    }

                    return $@"app.MapPost(""{path}"", {action});";
                });

            var source = $@"using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Micro.IoC
{{
    public static class EndpointExtensions
    {{
        public static void ConfigureMicroJsonContexts(this IServiceCollection services)
        {{

        }}

        public static void MapMicroEndpoints(this WebApplication app)
        {{
            {string.Join(@"
            ", endpoints)}
        }}
    }}
}}
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
