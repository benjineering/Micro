//using Micro.MinimalApi;
//using Microsoft.CodeAnalysis.CSharp.Testing;
//using Microsoft.CodeAnalysis.Testing.Verifiers;

//namespace Micro.Tests;

//public class SourceGeneratorTests
//{
//    [Fact]
//    public async Task SuckItAndSee()
//    {
//        var input = @"
//using Micro.Common.Requests;

//[RequestHandler]
//public static class Yes
//{    
//    static void Meow() { }
//}"
//        ;

//        var expected = @"namespace Plops
//{
//    const string MrMeow = ""meowers"";
//}";

//        var test = new CSharpSourceGeneratorTest<SourceGenerator, XUnitVerifier>
//        {
//            TestState =
//            {
//                Sources = { input },
//                GeneratedSources =
//                {
//                    (typeof(SourceGenerator), "Generated.g.cs", expected)
//                }
//            }
//        };

//        await test.RunAsync();


//    }
//}
