using Catalog.API.Models;
using Catalog.API.Products.CreateProduct;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.UpdateProduct
{

    public record UpdateProductReponse(Product Product);
    public record UpdateProductRequest(
        Guid Id,
        string Name,
        List<string> Categories,
        string Description,
        string ImageFile,
        decimal Price);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender Sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();
                var result = await Sender.Send(command);
                if (result is null)
                    return Results.Problem("Failed to update product.", statusCode: StatusCodes.Status500InternalServerError);
                return Results.Ok(new UpdateProductReponse(result.Product));
            })
            .WithName("UPDATE Product")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("UPDATE Product")
            .WithDescription("UPDATE Product"); ;
        }
    }
}
