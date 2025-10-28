using BugStore.Api.Responses.Products;
using MediatR;

namespace BugStore.Api.Requests.Products;

public record DeleteProductRequest(Guid Id) : IRequest<DeleteProductResponse>;