using Catalog.API.Models;
using Catalog.API.Products.CreateProduct;
using Catalog.API.Products.GetProducts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.GetProductById
{
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id:guid}", async ([FromRoute] Guid id, ISender sender) =>
            {
                try
                {
                    var query = new GetProductByIdQuery(id);
                    var result = await sender.Send(query);
                    var response = new GetProductByIdResponse(result.Product);

                    return Results.Ok(response);

                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.InnerException?.Message ?? ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "Get products failed");
                }
            })
             .WithName("get product by Id")
            .Produces<GetProductByIdEndPoint>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("get product by Id")
            .WithDescription("get product by Id");
        }
    }
}
