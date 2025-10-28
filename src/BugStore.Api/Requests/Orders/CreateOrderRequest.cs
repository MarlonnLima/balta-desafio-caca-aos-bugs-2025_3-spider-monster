using BugStore.Api.Models;
using BugStore.Api.Responses.Orders;
using MediatR;

namespace BugStore.Api.Requests.Orders;

public record CreateOrderRequest(Guid CustomerId, List<OrderLineRequest> Lines) : IRequest<CreateOrderResponse>;