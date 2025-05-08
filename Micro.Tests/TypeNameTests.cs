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
            string code = "namespace ANamespace { public class AClass { } }";
            var symbol = GetTypeSymbolFromSource(code, "AClass");

            var typeName = TypeName.FromSymbol(symbol);

            Assert.Equal("ANamespace.AClass", typeName.ToString());
        }

        [Fact]
        public void FromSymbol_Generics_Works()
        {
            string code = @"namespace TestNamespace2 { public class TestClass2 { } }
namespace TestNamespace3 { public class TestClass3 { } }
namespace TestNamespace.OhYeah { class GenericTestClass<A, B> { } }
namespace TestNamespace.OhYeah { class TestClass : GenericTestClass<TestNamespace2.TestClass2, TestNamespace3.TestClass3> { } }
";
            var symbol = GetTypeSymbolFromSource(code, "TestClass");

            var typeName = TypeName.FromSymbol(symbol);

            Assert.Equal("TestNamespace.OhYeah.TestClass<TestNamespace2.TestClass2, TestNamespace3.TestClass3>", typeName.ToString());
        }

        [Fact]
        public void FromSymbol_DeepGenerics_Works()
        {
            string code = @"namespace TestNamespace { public class TestClass2<T> { } }
namespace TestNamespace { public class TestClass3 { } }
namespace TestNamespace { class TestClass : TestClass2<TestClass3> { } }
";
            var symbol = GetTypeSymbolFromSource(code, "TestClass");

            var typeName = TypeName.FromSymbol(symbol);
            var typeStr = typeName.ToString();

            Assert.Equal("TestNamespace.TestClass<TestNamespace.TestClass2<TestNamespace.TestClass3>>", typeStr);
        }

        private static ITypeSymbol GetTypeSymbolFromSource(string sourceCode, string typeName)
        {
            var tree = CSharpSyntaxTree.ParseText(sourceCode);

            var compilation = CSharpCompilation.Create("Test")
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddSyntaxTrees(tree);

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
