using BugStore.Api.Models;

namespace BugStore.Api.Requests.Orders
{
    public record OrderLineRequest(int Quantity, Guid ProductId);
}
