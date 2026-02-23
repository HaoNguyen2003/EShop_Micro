using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.GetProductByCategory
{
    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;

    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    public class GetProductByCategoryHandler(IDocumentSession Session, ILogger<GetProductByCategoryHandler> logger) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = await Session.Query<Product>().Where(p => p.Categories.Contains(request.Category)).ToListAsync(cancellationToken);
            return new GetProductByCategoryResult(result);
        }
    }
}
