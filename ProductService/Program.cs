using MediatR;
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

var redisConfig = builder.Configuration.GetSection("Redis");

var redisOptions = new ConfigurationOptions
{
    EndPoints = { { redisConfig["Host"], int.Parse(redisConfig["Port"]) } },
    User = redisConfig["User"],
    Password = redisConfig["Password"],
    Ssl = true
};

//builder.Services.AddSingleton<IConnectionMultiplexer>(
//    ConnectionMultiplexer.Connect("localhost:6379"));
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