using MediatR;
using ProductService.Domain.Interfaces;
using ProductService.Features.Product.Commands;

namespace ProductService.Features.Product.Handlers;

public class UpdateInventoryHandler
    : IRequestHandler<UpdateInventoryCommand, bool>
{
    private readonly IProductRepository _repo;

    public UpdateInventoryHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(
        UpdateInventoryCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);

        if (product is null)
            return false;

        product.UpdateStock(request.Stock);

        await _repo.UpdateAsync(product);

        return true;
    }
}