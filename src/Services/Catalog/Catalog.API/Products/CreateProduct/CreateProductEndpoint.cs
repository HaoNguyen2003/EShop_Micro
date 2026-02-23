using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Products.CreateProduct
{

    public record CreateProductRequest(
        string Name,
        List<string> Categories,
        string Description,
        string ImageFile,
        decimal Price
    );
    public record CreateProductResponse(Guid Id);
    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async ([FromBody] CreateProductRequest request, [FromServices] ISender sender) =>
            {
                try
                {
                    var command = request.Adapt<CreateProductCommand>();
                    var result = await sender.Send(command);
                    if (result is null)
                        return Results.Problem("Failed to create product.", statusCode: StatusCodes.Status500InternalServerError);
                    var response = new CreateProductResponse(result.Id);
                    return Results.Created($"/products/{result.Id}", response);
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.InnerException?.Message ?? ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "Create product failed");
                }
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Product")
            .WithDescription("Creates Product");
        }
    }
}
