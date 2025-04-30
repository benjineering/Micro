using Micro.CodeGen.Models;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Micro.CodeGen.Generators
{
    interface IGenerator
    {
        void Generate(SourceProductionContext context, IEnumerable<Klass> klass);
    }
}
