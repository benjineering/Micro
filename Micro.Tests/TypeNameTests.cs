using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Micro.Common;

namespace Micro.Tests
{
    public class TypeNameTests
    {
        [Fact]
        public void FromSymbol_Works()
        {
            string code = @"namespace TestNamespace2 { class TestClass2 { } }
namespace TestNamespace3 { class TestClass3 { } }
namespace TestNamespace.OhYeah { class TestNamespace.OhYeah<TestNamespace2.TestClass2, TestNamespace3.TestClass3> { } }
";

            var symbol = GetTypeSymbolFromSource(code, "TestClass");

            var typeName = TypeName.FromSymbol(symbol);

            Assert.Equal("TestNamespace.OhYeah.TestNamespace.OhYeah<TestNamespace2.TestClass2, TestNamespace3.TestClass3>", typeName.ToString());
        }

        private static ITypeSymbol GetTypeSymbolFromSource(string sourceCode, string typeName)
        {
            var tree = CSharpSyntaxTree.ParseText(sourceCode);
            var compilation = CSharpCompilation.Create(
                "TestAssembly",
                [tree],
                [MetadataReference.CreateFromFile(typeof(object).Assembly.Location)],
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            var model = compilation.GetSemanticModel(tree);

            var root = tree.GetRoot();
            var classDecl = root.DescendantNodes()
                .OfType<TypeDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.Text == typeName) 
                ?? throw new InvalidOperationException($"Type '{typeName}' not found.");

            return model.GetDeclaredSymbol(classDecl) as ITypeSymbol
                ?? throw new InvalidOperationException("Failed to get ITypeSymbol.");
        }
    }
}
