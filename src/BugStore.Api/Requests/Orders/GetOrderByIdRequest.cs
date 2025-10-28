using BugStore.Api.Responses.Orders;
using MediatR;

namespace BugStore.Api.Requests.Orders;

public record GetOrderByIdRequest(Guid Id) : IRequest<GetOrderByIdResponse>;