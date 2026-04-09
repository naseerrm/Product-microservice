using MediatR;
using ProductService.Domain.Interfaces;
using ProductService.Features.Product.Commands;

namespace ProductService.Features.Product.Handlers;

public class UpdatePricingHandler
    : IRequestHandler<UpdatePricingCommand, bool>
{
    private readonly IProductRepository _repo;

    public UpdatePricingHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(
        UpdatePricingCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _repo.GetByIdAsync(request.Id);

        if (product is null)
            return false;

        product.UpdatePrice(request.Price);

        await _repo.UpdateAsync(product);

        return true;
    }
}