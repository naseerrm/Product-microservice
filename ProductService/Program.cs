using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Interfaces;
using ProductService.Features.Product.Endpoints;
using ProductService.Infrastructure.Caching;
using ProductService.Infrastructure.Repositories;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// DI
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductReadRepository>();
builder.Services.AddScoped<RedisCacheService>();

// Add first-level in-memory cache
builder.Services.AddMemoryCache();

var redisConfig = builder.Configuration.GetSection("Redis");

var host = redisConfig["Host"];
var port = redisConfig["Port"];
var user = redisConfig["User"];
var password = redisConfig["Password"];
var ssl = redisConfig["Ssl"];

var connectionString =
    $"{host}:{port}," +
    $"user={user}," +
    $"password={password}," +
    $"ssl={ssl}," +
    "abortConnect=False";

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var mux = ConnectionMultiplexer.Connect(connectionString);

    Console.WriteLine(mux.IsConnected
        ? "✅ Redis Connected"
        : "❌ Redis Failed");

    return mux;
});

// Add services to the container.
builder.Services.AddControllers();

// ✅ Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Minimal APIs

app.MapProductEndpoints();

app.Run();