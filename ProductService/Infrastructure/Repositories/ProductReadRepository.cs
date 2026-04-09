using Dapper;
using Microsoft.Data.SqlClient;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.Repositories;

public class ProductReadRepository
{
    private readonly IConfiguration _config;

    public ProductReadRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<Product>> SearchPagedAsync(
    string keyword,
    int pageNumber,
    int pageSize)
    {
        using var connection =
            new SqlConnection(
                _config.GetConnectionString("Default"));

        int offset = (pageNumber - 1) * pageSize;

        var sql = @"
        SELECT *
        FROM Products
        WHERE Name LIKE @Keyword
        ORDER BY Name
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY";

        return await connection.QueryAsync<Product>(
            sql,
            new
            {
                Keyword = $"%{keyword}%",
                Offset = offset,
                PageSize = pageSize
            });
    }

    public async Task<IEnumerable<Product>> SearchFastAsync(string keyword)
    {
        using var connection =
            new SqlConnection(
                _config.GetConnectionString("Default"));

        var sql = @"
            SELECT *
            FROM Products
            WHERE Name LIKE @Keyword";

        return await connection.QueryAsync<Product>(
            sql,
            new { Keyword = $"%{keyword}%" });
    }
}