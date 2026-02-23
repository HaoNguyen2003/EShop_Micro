
using Catalog.API.Products.CreateProduct;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.DeletedProduct
{
    public record DeletedProductByIdResponse(bool isSuccess);
    public class DeletedProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id:guid}", async ([FromRoute] Guid id, ISender sender) =>
            {
                try
                {
                    var query = new DeletedProductByIdQuery(id);
                    var result = await sender.Send(query);
                    var response = new DeletedProductByIdResponse(result.isSuccess);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.InnerException?.Message ?? ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "Delete product failed");
                }
            })
            .WithName("Delete Product")
            .Produces<DeletedProductByIdResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Product")
            .WithDescription("UPDelete Product"); ;
        }
    }
}
