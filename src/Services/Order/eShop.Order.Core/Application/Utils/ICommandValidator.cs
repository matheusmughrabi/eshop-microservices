using MediatR;

namespace eShop.Order.Core.Application.Utils;

public interface ICommandValidator<TCommand, TResult> where TCommand : IRequest<TResult>
{
    ValidationResult Validate(TCommand command);
}
