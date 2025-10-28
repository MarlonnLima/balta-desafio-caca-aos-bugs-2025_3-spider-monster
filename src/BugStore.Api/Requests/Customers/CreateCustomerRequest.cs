using BugStore.Api.Responses.Customers;
using MediatR;

namespace BugStore.Api.Requests.Customers;

public record CreateCustomerRequest(string Name, string Email, string Phone, DateTime BirthDate) : IRequest<CreateCustomerResponse>;