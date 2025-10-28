using BugStore.Api.Models;

namespace BugStore.Api.Responses.Orders;

public record GetOrderByIdResponse(Guid Id, Customer Customer, DateTime CreatedAt, DateTime UpdatedAt, List<OrderLine> Lines);