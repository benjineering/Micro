using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Micro.Server.Models
{
    public enum MicroDiagnosticType
    {
        ClassParserError,
        ClassGeneratorError,
        JsonContextGeneratorError,
        NotAClass,
        WrongReturnType,
    }

    class MicroDiagnostic
    {
        public bool IsWarning { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public static class MicroDiagnostics
    {
        // TODO: use nameof for namespaces and classes
        private static readonly Dictionary<MicroDiagnosticType, MicroDiagnostic> _diagnostics = new Dictionary<MicroDiagnosticType, MicroDiagnostic>
        {
            {
                MicroDiagnosticType.ClassGeneratorError,
                new MicroDiagnostic { Code = "UM001", IsWarning = false, Title = "Class parser threw an error" }
            },
            {
                MicroDiagnosticType.ClassGeneratorError,
                new MicroDiagnostic { Code = "UM100", IsWarning = false, Title = "Class generator threw an error" }
            },
            {
                MicroDiagnosticType.JsonContextGeneratorError,
                new MicroDiagnostic { Code = "UM101", IsWarning = false, Title = "JSON context generator threw an error" }
            },
            {
                MicroDiagnosticType.NotAClass,
                new MicroDiagnostic { Code = "UM200", IsWarning = false, Title = "Not a class", Message = "[RequestHandler] should only be applied to a class" }
            },
            {
                MicroDiagnosticType.WrongReturnType,
                new MicroDiagnostic { Code = "UM300", IsWarning = false, Title = "Wrong return type", Message = "Request handler methods must return Task<Micro.Response> or Micro.Response (with optional Response generic params)" }
            },
        };

        public static Diagnostic Create(MicroDiagnosticType type, IEnumerable<Location> locations = null, string messageOverride = null)
        {
            var firstLocation = locations?.FirstOrDefault();
            IEnumerable<Location> additionalLocations = locations?.Count() > 1 ? locations.Skip(1).ToArray() : null;

            if (!_diagnostics.TryGetValue(type, out var diagnostic))
            {
                return Diagnostic.Create(
                    new DiagnosticDescriptor("UM000", $"Unknown MicroDiagnosticType {type}", messageOverride ?? type.ToString(), nameof(Micro), DiagnosticSeverity.Error, isEnabledByDefault: true),
                    firstLocation,
                    additionalLocations);
            }

            var severity = diagnostic.IsWarning ? DiagnosticSeverity.Warning : DiagnosticSeverity.Error;

            return Diagnostic.Create(
                new DiagnosticDescriptor(diagnostic.Code, diagnostic.Title, messageOverride ?? diagnostic.Message, nameof(Micro), severity, isEnabledByDefault: true),
                firstLocation,
                additionalLocations);
        }
        public static Diagnostic[] CreateArray(MicroDiagnosticType type, IEnumerable<Location> locations = null, string messageOverride = null)
        {
            var diagnostic = Create(type, locations, messageOverride);
            return new Diagnostic[] { diagnostic };
        }
    }
}
