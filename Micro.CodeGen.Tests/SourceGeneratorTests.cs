using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace Micro.CodeGen.Tests;

public class SourceGeneratorTests
{
    [Fact]
    public async Task SuckItAndSee()
    {
        var input = @"
using Micro.Common.Requests;

public static class Yes
{    
    [RequestHandler]
    static void Meow() { }
}"
        ;

        var expected = @"namespace Plops
{
    const string MrMeow = ""meowers"";
}";

        var test = new CSharpSourceGeneratorTest<SourceGenerator, XUnitVerifier>
        {
            TestState =
            {
                Sources = { input },
                GeneratedSources =
                {
                    (typeof(SourceGenerator), "Test.g.cs", expected)
                }
            }
        };

        await test.RunAsync();


    }
}
