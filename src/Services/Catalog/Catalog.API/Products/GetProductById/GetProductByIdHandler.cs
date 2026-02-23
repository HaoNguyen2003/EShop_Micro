using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Microsoft.Extensions.Logging;

namespace Catalog.API.GetProductById
{
    public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
    public record GetProductByIdResult(Product Product);
    public class GetProductByIdQueryHandler(IDocumentSession Session, ILogger<GetProductByIdQueryHandler> logger) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
    {
        public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var result = Session.LoadAsync<Product>(query.Id, cancellationToken);
            if(result == null)
            {
                throw new ProductNotFoundException();
            }
            return new GetProductByIdResult(result.Result);
        }
    }
}
