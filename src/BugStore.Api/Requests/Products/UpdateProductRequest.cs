using BugStore.Api.Responses.Products;
using MediatR;

namespace BugStore.Api.Requests.Products;

public record UpdateProductRequest(string Title, string Description, string Slug, decimal Price) : IRequest<UpdateProductResponse>;