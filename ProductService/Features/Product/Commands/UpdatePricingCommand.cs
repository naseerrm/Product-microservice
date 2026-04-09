using MediatR;

namespace ProductService.Features.Product.Commands
{
    public record UpdatePricingCommand(Guid Id, decimal Price) : IRequest<bool>;
}
