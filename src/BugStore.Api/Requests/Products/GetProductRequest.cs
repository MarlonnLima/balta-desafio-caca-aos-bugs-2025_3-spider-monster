using BugStore.Api.Responses.Products;
using MediatR;

namespace BugStore.Api.Requests.Products;

public record GetProductRequest : IRequest<IEnumerable<GetProductResponse>>;