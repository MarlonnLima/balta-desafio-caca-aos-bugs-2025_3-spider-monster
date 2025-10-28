using BugStore.Api.Responses.Customers;
using MediatR;

namespace BugStore.Api.Requests.Customers;

public record DeleteCustomerRequest(Guid Id) : IRequest<DeleteCustomerResponse>;