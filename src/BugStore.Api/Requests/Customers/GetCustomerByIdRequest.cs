using BugStore.Api.Responses.Customers;
using MediatR;

namespace BugStore.Api.Requests.Customers;

public record GetCustomerByIdRequest(Guid Id) : IRequest<GetCustomerByIdResponse>;