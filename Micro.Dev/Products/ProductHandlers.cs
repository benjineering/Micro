namespace Micro.Dev.Products;

// TODO: dependency injection
[RequestHandler]
public class ProductHandlers
{
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
