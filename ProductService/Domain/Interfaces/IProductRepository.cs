using ProductService.Domain.Entities;

namespace ProductService.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> SearchAsync(string keyword);

        Task AddAsync(Product product);
        Task AddBulkAsync(IEnumerable<Product> products);

        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
