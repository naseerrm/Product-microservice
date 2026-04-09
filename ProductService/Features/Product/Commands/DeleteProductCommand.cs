using MediatR;

namespace ProductService.Features.Product.Commands
{
    public record DeleteProductCommand(Guid Id) : IRequest<bool>;
}
