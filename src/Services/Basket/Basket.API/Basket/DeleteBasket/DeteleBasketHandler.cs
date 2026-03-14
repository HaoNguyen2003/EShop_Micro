using Basket.API.Data;

namespace Basket.API.Basket.DeleteBasket
{

    public record DeleteBasketCommand(string userName) : ICommand<DeleteBasketResult>;
    public record DeleteBasketResult(bool IsSuccess);

    public class DeleteBasketCommandHandlerValidator : AbstractValidator<DeleteBasketCommand>
    {
        public DeleteBasketCommandHandlerValidator()
        {
            RuleFor(x => x.userName).NotEmpty().WithMessage("User name is request");
        }
    }
    public class DeteleBasketCommandHandler(IBasketRepository _basketRepository) : ICommandHandler<DeleteBasketCommand, DeleteBasketResult>
    {
        public async Task<DeleteBasketResult> Handle(DeleteBasketCommand command, CancellationToken cancellationToken)
        {
            string userName = command.userName;
            var result = await _basketRepository.DeleteBasket(userName, cancellationToken);
            return new DeleteBasketResult(result);
        }
    }
}
