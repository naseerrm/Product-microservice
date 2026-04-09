using MediatR;

namespace ProductService.Features.Product.Commands
{
    public record UpdateInventoryCommand(Guid Id, int Stock) : IRequest<bool>;
}
