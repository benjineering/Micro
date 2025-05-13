using Micro.Common;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Micro.Server.Generators
{
    interface IGenerator
    {
        void Generate(SourceProductionContext context, IEnumerable<Class> classes);
    }
}
