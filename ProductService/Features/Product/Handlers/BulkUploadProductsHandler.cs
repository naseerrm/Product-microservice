using MediatR;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Features.Product.Commands;


public class BulkUploadProductsHandler
    : IRequestHandler<BulkUploadProductsCommand, bool>
{
    private readonly IProductRepository _repo;

    public BulkUploadProductsHandler(IProductRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> Handle(
        BulkUploadProductsCommand request,
        CancellationToken cancellationToken)
    {
        var products = request.Products
            .Select(x => new Product(
                x.Name,
                x.Price,
                x.Stock))
            .ToList();

        await _repo.AddBulkAsync(products);

        return true;
    }
}