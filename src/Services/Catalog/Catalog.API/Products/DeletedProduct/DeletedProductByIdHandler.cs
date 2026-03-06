namespace Catalog.API.Products.DeletedProduct
{
    public record DeletedProductByIdCommand(Guid Id) : ICommand<DeletedProductByIdResult>;
    public record DeletedProductByIdResult(bool isSuccess);

    public class DeletedProductByIdValidator : AbstractValidator<DeletedProductByIdCommand>
    {
        public DeletedProductByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Product Id not null or empty");
        }
    }
    public class DeletedProductByIdCommandHandler(IDocumentSession Session) :
        ICommandHandler<DeletedProductByIdCommand, DeletedProductByIdResult>
    {
        public async Task<DeletedProductByIdResult> Handle(DeletedProductByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await Session.LoadAsync<Product>(request.Id, cancellationToken);
                if (product == null)
                {
                    throw new ProductNotFoundException(request.Id);
                }
                Session.Delete<Product>(product);
                await Session.SaveChangesAsync(cancellationToken);
                return new DeletedProductByIdResult(true);
            }
            catch (Exception ex)
            {
                return new DeletedProductByIdResult(false);
            }
        }
    }
}
