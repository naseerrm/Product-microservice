using MediatR;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Repositories;

public class SearchProductsPagedHandler
    : IRequestHandler<SearchProductsPagedQuery, IEnumerable<Product>>
{
    private readonly ProductReadRepository _readRepo;

    public SearchProductsPagedHandler(
        ProductReadRepository readRepo)
    {
        _readRepo = readRepo;
    }

    public async Task<IEnumerable<Product>> Handle(
        SearchProductsPagedQuery request,
        CancellationToken cancellationToken)
    {
        return await _readRepo.SearchPagedAsync(
            request.Keyword,
            request.PageNumber,
            request.PageSize);
    }
}