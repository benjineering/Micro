using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.RuntimeSupport;
using System.Text.Json.Serialization;

namespace Micro.Dev.Lambda;

public class Function
{
    //public async Task FunctionHandler(Stream inputStream, Stream outputStream, ILambdaContext context)
    public string FunctionHandler(MyRequest input, ILambdaContext context)
    {
        return $"Hello, {input.Name}! From .NET 9 AoT on Graviton4.";

        //FunctionRequest request;
        //try
        //{
        //  request = (await JsonSerializer.DeserializeAsync<FunctionRequest>(inputStream))!;
        //  if (request == null)
        //    throw new Exception("Request deserialized to null");
        //}
        //catch (Exception ex)
        //{
        //  var errorMessage = $"Request is not a valid FunctionRequest\n\t{ex.Message}";
        //  context.Logger.LogCritical(errorMessage);

        //  var response = Response.Error(HttpStatusCode.BadRequest, errorMessage);
        //  await JsonSerializer.SerializeAsync(outputStream, response);
        //}

        // TODO: somehow call the method
    }
}

public class MyRequest
{
    public string Name { get; set; }
}
[JsonSerializable(typeof(MyRequest))]
partial class LambdaJsonContext : JsonSerializerContext { }

public class Program
{
    static async Task Main()
    {
        // Create a Lambda runtime with our function handler
        static string handler(MyRequest input, ILambdaContext context)
        {
            return new Function().FunctionHandler(input, context);
        }

        var serializer = new SourceGeneratorLambdaJsonSerializer<LambdaJsonContext>();
        using var handlerWrapper = HandlerWrapper.GetHandlerWrapper((Func<MyRequest, ILambdaContext, string>)handler, serializer);
        using var bootstrap = new LambdaBootstrap(handlerWrapper);

        await bootstrap.RunAsync();
    }
}
