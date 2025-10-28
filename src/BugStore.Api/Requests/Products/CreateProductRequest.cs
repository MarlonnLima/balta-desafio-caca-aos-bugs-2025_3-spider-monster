using MediatR;
using BugStore.Api.Responses.Products;
namespace BugStore.Api.Requests.Products;

public record CreateProductRequest(string Title, string Description, string Slug, decimal Price) : IRequest<CreateProductResponse>;