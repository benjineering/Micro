using Microsoft.CodeAnalysis;

namespace Micro.CodeGen.Models
{
    class MethodParserResult
    {
        public Method Method { get; set; }

        public Diagnostic Diagnostic { get; set; }
    }
}
