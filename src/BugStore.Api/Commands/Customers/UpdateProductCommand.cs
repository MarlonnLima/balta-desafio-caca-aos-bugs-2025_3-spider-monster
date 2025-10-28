using BugStore.Api.Requests.Customers;
using BugStore.Api.Responses.Customers;
using MediatR;

namespace BugStore.Api.Commands.Customers
{
    public record UpdateCustomerCommand(Guid Id, UpdateCustomerRequest Data) : IRequest<UpdateCustomerResponse>;
}
