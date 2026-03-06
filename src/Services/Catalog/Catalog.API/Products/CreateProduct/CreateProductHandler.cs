namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(
        string Name,
        List<string> Categories,
        string Description,
        string ImageFile,
        decimal Price
    ) : ICommand<CreateProductResult>;

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Categories).NotEmpty().WithMessage("Category is Required");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Category is Required");
            RuleFor(x => x.ImageFile).MaximumLength(200);
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        }

    }
    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler
        (IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = command.Name,
                Categories = command.Categories,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(product.Id);
        }
    }
}
