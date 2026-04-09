using MediatR;
using ProductService.Features.Product.Commands;
namespace ProductService.Features.Product.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        // Create
        app.MapPost("/products",
            async (CreateProductCommand cmd, IMediator mediator) =>
                Results.Ok(await mediator.Send(cmd)));

        // Update
        app.MapPut("/products",
            async (UpdateProductCommand cmd, IMediator mediator) =>
                Results.Ok(await mediator.Send(cmd)));

        // Delete
        app.MapDelete("/products/{id}",
            async (Guid id, IMediator mediator) =>
                Results.Ok(await mediator.Send(
                    new DeleteProductCommand(id))));

        // Get By Id
        app.MapGet("/products/{id}",
            async (Guid id, IMediator mediator) =>
                Results.Ok(await mediator.Send(
                    new GetProductByIdQuery(id))));

        // Search
        app.MapGet("/products/search/{keyword}",
            async (string keyword, IMediator mediator) =>
                Results.Ok(await mediator.Send(
                    new SearchProductsQuery(keyword))));

        // Bulk Upload
        app.MapPost("/products/bulk-upload",
            async (BulkUploadProductsCommand cmd, IMediator mediator) =>
                Results.Ok(await mediator.Send(cmd)));

        // Inventory
        app.MapPut("/products/inventory",
            async (UpdateInventoryCommand cmd, IMediator mediator) =>
                Results.Ok(await mediator.Send(cmd)));

        // Pricing
        app.MapPut("/products/pricing",
            async (UpdatePricingCommand cmd, IMediator mediator) =>
                Results.Ok(await mediator.Send(cmd)));

        app.MapGet("/products/paged-search",
    async (
        string keyword,
        int pageNumber,
        int pageSize,
        IMediator mediator) =>
    {
        return Results.Ok(
            await mediator.Send(
                new SearchProductsPagedQuery(
                    keyword,
                    pageNumber,
                    pageSize)));
    });
    }
}