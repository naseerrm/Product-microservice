using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace ProductService.Infrastructure.Caching;

public class RedisCacheService
{
    private readonly IDatabase _db;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly TimeSpan _memoryTtl = TimeSpan.FromSeconds(30);   // fast first-level cache
    private readonly TimeSpan _redisQuickTimeout = TimeSpan.FromMilliseconds(400); // fail-fast threshold

    public RedisCacheService(
        IConnectionMultiplexer redis,
        IMemoryCache memoryCache,
        ILogger<RedisCacheService> logger)
    {
        _db = redis.GetDatabase();
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        // 1) First-level in-memory cache (very fast, avoids network)
        if (_memoryCache.TryGetValue<T>(key, out var cachedMem))
            return cachedMem;

        // 2) Try Redis but fail fast if it is slow/unavailable
        try
        {
            var valueTask = _db.StringGetAsync(key);
            var completed = await Task.WhenAny(valueTask, Task.Delay(_redisQuickTimeout));
            if (completed != valueTask)
            {
                _logger.LogWarning("Redis GET timed out (fail-fast) for key {Key}", key);
                return default;
            }

            var value = await valueTask;
            if (value.IsNullOrEmpty)
                return default;

            var obj = JsonSerializer.Deserialize<T>(value.ToString());
            if (obj is not null)
                _memoryCache.Set(key, obj, _memoryTtl);

            return obj;
        }
        catch (StackExchange.Redis.RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Redis connection failed while GET for key {Key}", key);
            return default;
        }
        catch (StackExchange.Redis.RedisTimeoutException ex)
        {
            _logger.LogWarning(ex, "Redis timeout while GET for key {Key}", key);
            return default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while GET for key {Key}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value)
    {
        // 1) Update first-level memory cache quickly
        _memoryCache.Set(key, value, _memoryTtl);

        // 2) Fire async Redis set but fail fast / log if it takes too long
        try
        {
            var setTask = _db.StringSetAsync(
                key,
                JsonSerializer.Serialize(value),
                TimeSpan.FromMinutes(5));

            var completed = await Task.WhenAny(setTask, Task.Delay(_redisQuickTimeout));
            if (completed != setTask)
            {
                _logger.LogWarning("Redis SET timed out (fail-fast) for key {Key}", key);
                return;
            }

            // observe result to propagate potential exceptions
            await setTask;
        }
        catch (StackExchange.Redis.RedisConnectionException ex)
        {
            _logger.LogWarning(ex, "Redis connection failed while SET for key {Key}", key);
        }
        catch (StackExchange.Redis.RedisTimeoutException ex)
        {
            _logger.LogWarning(ex, "Redis timeout while SET for key {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while SET for key {Key}", key);
        }
    }
}