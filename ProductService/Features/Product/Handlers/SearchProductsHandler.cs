using MediatR;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Repositories;


public class SearchProductsHandler
    : IRequestHandler<SearchProductsQuery, IEnumerable<Product>>
{
    private readonly ProductReadRepository _readRepo;

    public SearchProductsHandler(ProductReadRepository readRepo)
    {
        _readRepo = readRepo;
    }

    public async Task<IEnumerable<Product>> Handle(
        SearchProductsQuery request,
        CancellationToken cancellationToken)
    {
        return await _readRepo.SearchFastAsync(request.Keyword);
    }
}