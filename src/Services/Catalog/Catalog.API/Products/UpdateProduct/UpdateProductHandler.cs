using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Products.UpdateProduct
{

    public record UpdateProductCommand
        (
        Guid Id,
        string Name,
        List<string> Categories,
        string Description,
        string ImageFile,
        decimal Price) : ICommand<UpdateProductResult>;

    public record UpdateProductResult(Product Product);

    public class UpdateProductCommandHandler(IDocumentSession Session, ILogger<UpdateProductCommandHandler> logger) 
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var product = await Session.LoadAsync<Product>(command.Id, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException();
            }
            product.Name = command.Name;
            product.Description = command.Description;
            product.Price = command.Price;
            product.ImageFile = command.ImageFile;
            product.Categories = command.Categories;
            Session.Update(product);
            await Session.SaveChangesAsync(cancellationToken);
            return new UpdateProductResult(product);
        }
    }
}
