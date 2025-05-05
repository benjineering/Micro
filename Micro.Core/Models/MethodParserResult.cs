using Micro.Common;
using Microsoft.CodeAnalysis;

namespace Micro.Core.Models
{
    class MethodParserResult
    {
        public Method Method { get; set; }

        public Diagnostic Diagnostic { get; set; }
    }
}
