using Microsoft.CodeAnalysis;
using Micro.Common;

namespace Micro.Core.Models
{
    class ClassParserResult
    {
        public Class Class { get; set; } = null;

        public Diagnostic[] Diagnostics { get; set; } = new Diagnostic[0];
    }
}
