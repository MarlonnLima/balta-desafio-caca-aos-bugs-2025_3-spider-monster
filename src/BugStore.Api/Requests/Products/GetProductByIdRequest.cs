using BugStore.Api.Responses.Products;
using MediatR;

namespace BugStore.Api.Requests.Products;

public record GetProductByIdRequest(Guid Id) : IRequest<GetProductByIdResponse>;