using Catalog.API.Models;
using Catalog.API.Products.DeletedProduct;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/by-category", async (
                [FromQuery] string category, [FromServices] ISender sender) =>
            {
                try
                {
                    var query = new GetProductByCategoryQuery(category);
                    var result = await sender.Send(query);
                    var response = new GetProductByCategoryResponse(result.Products);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.InnerException?.Message ?? ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "Get products by category failed");
                }
            })
            .WithName("GetProductByCategoryResponse Product")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("GetProductByCategoryResponse Product")
            .WithDescription("UPDelete Product"); ;
        }
    }
}
