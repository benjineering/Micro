using Microsoft.CodeAnalysis;

namespace Micro.Common
{
    public class MethodParserResult
    {
        public Method Method { get; set; }

        public Diagnostic Diagnostic { get; set; }
    }
}
