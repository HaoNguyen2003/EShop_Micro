using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Catalog.API.Products.CreateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.GetProducts
{

    public record GetProductsResponse(IEnumerable<Product> Products);


    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([FromServices] ISender sender) =>
            {
                try
                {
                    var result = await sender.Send(new GetProductsQuery());
                    var response = result.Adapt<GetProductsResponse>();

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
            .WithName("GetProduct")
            .Produces<GetProductsResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product")
            .WithDescription("Get Product"); ;
        }
    }
}
