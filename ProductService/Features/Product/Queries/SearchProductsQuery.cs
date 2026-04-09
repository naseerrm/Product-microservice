using MediatR;
using ProductService.Domain.Entities;

public record SearchProductsQuery(string Keyword) : IRequest<IEnumerable<Product>>;
