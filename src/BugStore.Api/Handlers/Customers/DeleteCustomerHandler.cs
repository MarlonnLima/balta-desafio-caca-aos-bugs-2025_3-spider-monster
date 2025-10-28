using BugStore.Api.Data;
using BugStore.Api.Requests.Customers;
using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Customers;
using BugStore.Api.Responses.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Api.Handlers.Customers
{
    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerRequest, DeleteCustomerResponse>
    {
        private readonly AppDbContext _dbContext;

        public DeleteCustomerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeleteCustomerResponse?> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (customer == null)
                return null;

            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteCustomerResponse(customer.Id);
        }
    }
}
