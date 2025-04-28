using Microsoft.CodeAnalysis;

namespace Micro.CodeGen.Models
{
    class Method
    {
        public string Name { get; set; }

        public IParameterSymbol[] Parameters { get; set; }

        public ITypeSymbol ReturnType { get; set; }
    }
}
