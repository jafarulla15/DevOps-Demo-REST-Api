using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("Angular");

app.MapGet("/", () => Results.Ok(new
{
    message = "Demo .NET 8 REST API is running",
    status = "healthy"
}));

app.MapGet("/api/products", () =>
{
    var products = new[]
    {
        new Product(1, "Laptop", 85000),
        new Product(2, "Keyboard", 3500),
        new Product(3, "Mouse", 1800),
        new Product(4, "Monitor", 28000)
    };

    return Results.Ok(products);
});

app.MapGet("/api/products/{id:int}", (int id) =>
{
    var products = new[]
    {
        new Product(1, "Laptop", 85000),
        new Product(2, "Keyboard", 3500),
        new Product(3, "Mouse", 1800),
        new Product(4, "Monitor", 28000)
    };

    var product = products.FirstOrDefault(x => x.Id == id);

    return product is null
        ? Results.NotFound(new { message = $"Product {id} not found" })
        : Results.Ok(product);
});

app.MapPost("/api/products", ([FromBody] CreateProductRequest request) =>
{
    var product = new Product(
        Random.Shared.Next(100, 999),
        request.Name,
        request.Price);

    return Results.Created($"/api/products/{product.Id}", product);
});

app.Run();

public record Product(int Id, string Name, decimal Price);

public record CreateProductRequest(string Name, decimal Price);
