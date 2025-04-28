using Micro.Requests;
using Micro.Responses;

namespace Micro.Dev.Products;

// TODO: dependency injection
public class ProductHandlers
{
    [RequestHandler]
    public async Task<Response> GetProductDetail(Guid id)
    {
        return Response.Success(new 
        {
            Id = id,
            Name = "Ploppin' Product",
            CreatedAt = DateTime.UtcNow,
        });
    }
}
