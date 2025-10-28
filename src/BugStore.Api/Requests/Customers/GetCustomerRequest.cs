using BugStore.Api.Responses.Customers;
using BugStore.Api.Responses.Products;
using MediatR;

namespace BugStore.Api.Requests.Customers;

public record GetCustomerRequest() : IRequest<IEnumerable<GetCustomerResponse>>;