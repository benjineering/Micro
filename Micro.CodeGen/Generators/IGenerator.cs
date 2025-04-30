using Micro.CodeGen.Models;
using System.Collections.Generic;

namespace Micro.CodeGen.Generators
{
    interface IGenerator
    {
        string Generate(IEnumerable<Klass> klass);
    }
}
