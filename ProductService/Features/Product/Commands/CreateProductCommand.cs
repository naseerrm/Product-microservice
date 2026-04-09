using MediatR;

namespace ProductService.Features.Product.Commands;

public record CreateProductCommand(string Name, decimal Price,int Stock)
    : IRequest<Guid>;