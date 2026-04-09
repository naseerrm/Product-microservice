using MediatR;
using ProductService.Domain.Interfaces;
using ProductService.Features.Product.Commands;

namespace ProductService.Features.Product.Handlers;

public class UpdateProductHandler
    : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductRepository _repo;

    public UpdateProductHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);

        if (product is null) return false;

        product.Update(
            request.Name,
            request.Price,
            request.Stock);

        await _repo.UpdateAsync(product);

        return true;
    }
}