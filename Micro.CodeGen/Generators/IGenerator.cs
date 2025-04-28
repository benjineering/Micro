using Micro.CodeGen.Models;

namespace Micro.CodeGen.Generators
{
    interface IGenerator
    {
        string Generate(Klass klass);
    }
}
