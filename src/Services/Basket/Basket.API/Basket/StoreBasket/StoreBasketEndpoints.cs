
using Basket.API.Basket.GetBasket;

namespace Basket.API.Basket.StoreBasket
{

    public record StoreBasketRequest(ShoppingCart Cart);

    public record StoreBasketResponse(string UserName);

    public class StoreBasketEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                var result = await sender.Send(new StoreBasketCommand(request.Cart));
                var response = result.Adapt<StoreBasketResponse>();
                return Results.Ok(response);
            })
            .WithName("StoreBasketEndpoints")
            .Produces<StoreBasketEndpoints>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("StoreBasketEndpoints")
            .WithDescription("StoreBasketEndpoints");
        }
    }
}
