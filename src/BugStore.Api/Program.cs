using BugStore.Api.Commands.Products;
using BugStore.Api.Data;
using BugStore.Api.Requests.Products;
using BugStore.Api.Requests.Customers;
using BugStore.Api.Requests.Orders;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BugStore.Api.Commands.Customers;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddMediatR(c =>
{
    c.RegisterServicesFromAssemblyContaining<Program>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

#region Customers
    app.MapGet("/v1/customers", async (IMediator mediator) =>
    {
        var result = await mediator.Send(new GetCustomerRequest());
        return Results.Ok(result);
    });

    app.MapGet("/v1/customers/{id}", async (IMediator mediator, Guid id) =>
    {
        var result = await mediator.Send(new GetCustomerByIdRequest(id));
        return result is not null ? Results.Ok(result) : Results.NotFound("Cliente não encontrado.");
    });

    app.MapPost("/v1/customers", async (IMediator mediator, [FromBody] CreateCustomerRequest request) =>
    {
        var result = await mediator.Send(request);
        return Results.Ok(result);
    });

    app.MapPut("/v1/customers/{id}", async (IMediator mediator, Guid id, [FromBody] UpdateCustomerRequest request) =>
    {
        var result = await mediator.Send(new UpdateCustomerCommand(id, request));
        return result is not null ? Results.Ok(result) : Results.NotFound("Cliente não encontrado.");
    });

    app.MapDelete("/v1/customers/{id}", async (IMediator mediator, Guid id) =>
    {
        var result = await mediator.Send(new DeleteCustomerRequest(id));
        return result is not null ? Results.Ok(result) : Results.NotFound("Cliente não encontrado.");
    });

#endregion

#region Products
    app.MapGet("/v1/products", async (IMediator mediator) =>
    {
        var result = await mediator.Send(new GetProductRequest());
        return Results.Ok(result);
    });

    app.MapGet("/v1/products/{id}", async (IMediator mediator, Guid id) =>
    {
        var result = await mediator.Send(new GetProductByIdRequest(id));
        return result is not null ? Results.Ok(result) : Results.NotFound("Produto não encontrado.");
    });

    app.MapPost("/v1/products", async (IMediator mediator, [FromBody] CreateProductRequest request) =>
    {
        var result = await mediator.Send(request);
        return Results.Ok(result);
    });

    app.MapPut("/v1/products/{id}", async (IMediator mediator, Guid id, [FromBody] UpdateProductRequest request) =>
    {
        var result = await mediator.Send(new UpdateProductCommand(id, request));

        return result is not null ? Results.Ok(result) : Results.NotFound("Produto não encontrado.");
    });
    app.MapDelete("/v1/products/{id}", async (IMediator meaditor, Guid id) =>
    {
        var result = await meaditor.Send(new DeleteProductRequest(id));
        return result is not null ? Results.Ok(result) : Results.NotFound("Produto não encontrado.");
    });
#endregion

#region Orders 
    app.MapGet("/v1/orders/{id}", async (IMediator mediator, Guid id) =>
    {
        var result = await mediator.Send(new GetOrderByIdRequest(id));
        return Results.Ok(result);
    });
    app.MapPost("/v1/orders", async (IMediator mediator, [FromBody] CreateOrderRequest request) =>
    {
        var result = await mediator.Send(request);
        return Results.Ok(result);
    });
#endregion

app.Run();
