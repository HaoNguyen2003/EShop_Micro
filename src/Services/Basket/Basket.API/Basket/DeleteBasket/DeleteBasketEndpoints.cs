namespace Basket.API.Basket.DeleteBasket
{

    public record DeteleBasketResponse(bool IsSuccess);
    public class DeleteBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(userName));
                var response = result.Adapt<DeteleBasketResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteBasketEndpoints")
            .Produces<DeleteBasketEndpoints>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("DeleteBasketEndpoints")
            .WithDescription("DeleteBasketEndpoints");
        }
    }
}
