using Microsoft.CodeAnalysis;

namespace Micro.Server.Models
{
    public class MethodParserResult
    {
        public Method Method { get; set; }

        public Diagnostic Diagnostic { get; set; }
    }
}
