using MediatR;
using ProductService.Domain.Entities;
using ProductService.Domain.Interfaces;
using ProductService.Infrastructure.Caching;


public class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, Product>
{
    private readonly IProductRepository _repo;
    private readonly RedisCacheService _cache;

    public GetProductByIdHandler( IProductRepository repo, RedisCacheService cache)
    {
        _repo = repo;
        _cache = cache;
    }

    public async Task<Product> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        string key = $"product:{request.Id}";

        var cached =
            await _cache.GetAsync<Product>(key);

        if (cached is not null)
            return cached;

        var product =
            await _repo.GetByIdAsync(request.Id);

        if (product is not null)
            await _cache.SetAsync(key, product);

        return product;
    }
}