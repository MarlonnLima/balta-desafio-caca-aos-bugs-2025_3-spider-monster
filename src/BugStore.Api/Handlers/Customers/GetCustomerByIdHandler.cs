using BugStore.Api.Data;
using BugStore.Api.Requests.Customers;
using BugStore.Api.Responses.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Api.Handlers.Customers
{
    public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdRequest, GetCustomerByIdResponse>
    {
        private readonly AppDbContext _dbContext;

        public GetCustomerByIdHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetCustomerByIdResponse?> Handle(GetCustomerByIdRequest request, CancellationToken cancellationToken)
        {
            var customer = _dbContext.Customers
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == request.Id);

            if (customer == null)
                return null;

            return new GetCustomerByIdResponse(customer.Id, customer.Name, customer.Email, customer.Phone, customer.BirthDate);
        }
    }
}
