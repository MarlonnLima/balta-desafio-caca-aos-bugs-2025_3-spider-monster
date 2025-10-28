using BugStore.Api.Data;
using BugStore.Api.Models;
using BugStore.Api.Requests.Customers;
using BugStore.Api.Responses.Customers;
using MediatR;

namespace BugStore.Api.Handlers.Customers
{
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, CreateCustomerResponse>
    {
        private readonly AppDbContext _dbContext;

        public CreateCustomerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateCustomerResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                BirthDate = request.BirthDate
            };

            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateCustomerResponse(customer.Id);
        }
    }
}
