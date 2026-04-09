using MediatR;
using ProductService.Domain.Entities;

public record SearchProductsPagedQuery(
    string Keyword,
    int PageNumber,
    int PageSize)
    : IRequest<IEnumerable<Product>>;