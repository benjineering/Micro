using Microsoft.CodeAnalysis;

namespace Micro.CodeGen.Models
{
    class ClassParserResult
    {
        public Class Class { get; set; } = null;

        public Diagnostic[] Diagnostics { get; set; } = new Diagnostic[0];
    }
}
