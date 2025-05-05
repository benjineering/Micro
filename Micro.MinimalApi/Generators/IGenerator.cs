using Micro.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Micro.MinimalApi.Generators
{
    interface IGenerator
    {
        void Generate(SourceProductionContext context, IEnumerable<Class> classes);
    }
}
