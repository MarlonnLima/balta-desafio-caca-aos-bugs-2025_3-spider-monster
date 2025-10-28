using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Products;
using MediatR;

namespace BugStore.Api.Commands.Products
{
    public record UpdateProductCommand(Guid Id,  UpdateProductRequest Data) : IRequest<UpdateProductResponse>;
}
