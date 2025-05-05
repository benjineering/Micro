using System.Net;

namespace Micro
{
    public abstract class Response
    {
        public HttpStatusCode? StatusCode { get; } = HttpStatusCode.OK;

        public Response(HttpStatusCode? statusCode = HttpStatusCode.OK)
        {
            StatusCode = statusCode;
        }

        public static SuccessResponse Success(HttpStatusCode? statusCode = HttpStatusCode.OK)
        {
            return new SuccessResponse(statusCode);
        }

        public static SuccessResponse<T> Success<T>(T value = default, HttpStatusCode? statusCode = HttpStatusCode.OK)
        {
            return new SuccessResponse<T>(value, statusCode);
        }

        public static ErrorResponse<TError> Error<TError>(HttpStatusCode statusCode, string errorMessage = null, TError errorObject = default)
        {
            return new ErrorResponse<TError>(statusCode, errorMessage, errorObject);
        }

        public static ErrorResponse<TValue, TError> Error<TValue, TError>(HttpStatusCode statusCode, string errorMessage = null, TError errorObject = default)
        {
            return new ErrorResponse<TValue, TError>(statusCode, errorMessage, errorObject);
        }
    }

    public class SuccessResponse : Response
    {
        public SuccessResponse(HttpStatusCode? statusCode = HttpStatusCode.OK)
            : base(statusCode) { }
    }

    public class SuccessResponse<T> : Response
    {
        public T Value { get; }

        public SuccessResponse(T value = default, HttpStatusCode? statusCode = HttpStatusCode.OK)
            : base(statusCode)
        {
            Value = value;
        }
    }

    public class ErrorResponse<TError> : Response
    {
        public string ErrorMessage { get; }

        public TError ErrorObject { get; }

        public ErrorResponse(HttpStatusCode statusCode, string errorMessage = null, TError errorObject = default)
            : base(statusCode)
        {
            ErrorMessage = errorMessage;
            ErrorObject = errorObject;
        }
    }

    public class ErrorResponse<TValue, TError> : ErrorResponse<TError>
    {
        public ErrorResponse(HttpStatusCode statusCode, string errorMessage = null, TError errorObject = default)
            : base(statusCode, errorMessage, errorObject) { }
    }
}
