using MediatR;
using ProductService.Domain.Entities;


public record GetProductByIdQuery(Guid Id) : IRequest<Product>;