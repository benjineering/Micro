using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;

namespace Micro.CodeGen.Tests;

public class SvelteKitSourceGeneratorTests
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

        var test = new CSharpSourceGeneratorTest<SvelteKitSourceGenerator, XUnitVerifier>
        {
            TestState =
            {
                Sources = { input },
                GeneratedSources =
                {
                    (typeof(SvelteKitSourceGenerator), "Test.g.cs", expected)
                }
            }
        };

        await test.RunAsync();


    }
}
