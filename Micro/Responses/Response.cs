using System.Net;

namespace Micro.Responses;

public abstract record Response
{
    public static SuccessResponse Success(dynamic? Value = null, HttpStatusCode? StatusCode = HttpStatusCode.OK)
    {
        return new SuccessResponse(Value, StatusCode);
    }
    public static ErrorResponse Error(HttpStatusCode StatusCode, string? Message = null, dynamic? ErrorObject = null)
    {
        return new ErrorResponse(StatusCode, Message, ErrorObject);
    }
}

public record SuccessResponse(dynamic? Value = null, HttpStatusCode? StatusCode = HttpStatusCode.OK) : Response;

public record ErrorResponse(HttpStatusCode StatusCode, string? Message = null, dynamic? ErrorObject = null) : Response;
