namespace Micro.Requests;

public record FunctionRequest
{
    public required string MethodName { get; init; }
}
