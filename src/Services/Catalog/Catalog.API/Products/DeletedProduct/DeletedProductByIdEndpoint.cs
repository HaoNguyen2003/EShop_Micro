namespace Catalog.API.Products.DeletedProduct
{
    public record DeletedProductByIdResponse(bool isSuccess);
    public class DeletedProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new DeletedProductByIdCommand(id);
                var result = await sender.Send(query);
                var response = new DeletedProductByIdResponse(result.isSuccess);
                return Results.Ok(response);
            })
            .WithName("Delete Product")
            .Produces<DeletedProductByIdResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Product")
            .WithDescription("UPDelete Product"); ;
        }
    }
}
