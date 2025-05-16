using Micro.Server.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Micro.Server.Generators
{
    class LambdaGenerator
    {
        public void Generate(SourceProductionContext context, IEnumerable<Class> classes)
        {
            foreach (var klass in classes)
            {
                var source = $@"namespace Micro.Internal.{klass.Name.Namespace}
{{
    class {klass.Name.Name}
    {{
        // TODO
    }}
}}
";

                context.AddSource($@"{klass.Name}.g.cs", source);
            }
        }
    }
}
