using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.GetProductById;
using Catalog.API.Models;

namespace Catalog.API.Products.DeletedProduct
{
    public record DeletedProductByIdQuery(Guid Id) : IQuery<DeletedProductByIdResult>;
    public record DeletedProductByIdResult(bool isSuccess);
    public class DeletedProductByIdQueryHandler(IDocumentSession Session, ILogger<GetProductByIdQueryHandler> logger) : IQueryHandler<DeletedProductByIdQuery, DeletedProductByIdResult>
    {
        public async Task<DeletedProductByIdResult> Handle(DeletedProductByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await Session.LoadAsync<Product>(request.Id, cancellationToken);
                if (product == null)
                {
                    throw new ProductNotFoundException();
                }
                Session.Delete<Product>(product);
                await Session.SaveChangesAsync(cancellationToken);
                return new DeletedProductByIdResult(true);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Remove fail", request.Id);
                return new DeletedProductByIdResult(false);
            }
        }
    }
}
