using BugStore.Api.Commands.Customers;
using BugStore.Api.Data;
using BugStore.Api.Responses.Customers;
using MediatR;

namespace BugStore.Api.Handlers.Customers
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResponse>
    {
        private readonly AppDbContext _dbContext;

        public UpdateCustomerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateCustomerResponse?> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _dbContext
                .Customers
                .FirstOrDefault(p => p.Id == request.Id);

            if (customer == null)
                return null;

            customer.Name = request.Data.Name;
            customer.Email = request.Data.Email;
            customer.Phone = request.Data.Phone;
            customer.BirthDate = request.Data.BirthDate;

            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateCustomerResponse();
        }
    }
}
