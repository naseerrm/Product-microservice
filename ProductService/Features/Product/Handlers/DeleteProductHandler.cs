using MediatR;
using ProductService.Domain.Interfaces;
using ProductService.Features.Product.Commands;

namespace ProductService.Features.Product.Handlers;

public class DeleteProductHandler
    : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _repo;

    public DeleteProductHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);

        if (product is null) return false;

        await _repo.DeleteAsync(product);

        return true;
    }
}