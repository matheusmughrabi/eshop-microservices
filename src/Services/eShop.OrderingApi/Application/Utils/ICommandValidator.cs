using MediatR;

namespace eShop.OrderingApi.Application.Utils;

public interface ICommandValidator<TCommand, TResult> where TCommand : IRequest<TResult>
{
    ValidationResult Validate(TCommand command);
}
