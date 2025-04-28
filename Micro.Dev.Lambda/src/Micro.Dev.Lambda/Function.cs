using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.RuntimeSupport;

namespace Micro.Dev.Lambda;

public class Function
{
    //public async Task FunctionHandler(Stream inputStream, Stream outputStream, ILambdaContext context)
    public string FunctionHandler(string input, ILambdaContext context)
    {
        return $"Hello, {input}! From .NET 9 AoT on Graviton4.";

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
public class Program
{
    static async Task Main(string[] args)
    {
        // Create a Lambda runtime with our function handler
        static string handler(string input, ILambdaContext context)
        {
            return new Function().FunctionHandler(input, context);
        }

        // TODO: var serializer = new SourceGeneratorLambdaJsonSerializer<HelloLambdaAotJsonContext>();
        var serializer = new DefaultLambdaJsonSerializer();
        using var handlerWrapper = HandlerWrapper.GetHandlerWrapper((Func<string, ILambdaContext, string>)handler, serializer);
        using var bootstrap = new LambdaBootstrap(handlerWrapper);

        await bootstrap.RunAsync();
    }
}
