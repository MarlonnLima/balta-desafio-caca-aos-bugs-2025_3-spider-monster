using BugStore.Api.Data;
using BugStore.Api.Requests.Customers;
using BugStore.Api.Responses.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Api.Handlers.Customers
{
    public class GetCustomerHandler : IRequestHandler<GetCustomerRequest, IEnumerable<GetCustomerResponse>>
    {
        private readonly AppDbContext _dbContext;

        public GetCustomerHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GetCustomerResponse>> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
        {
            var customers = await _dbContext.Customers
                .AsNoTracking()
                .Select(p => new GetCustomerResponse(p.Id, p.Name, p.Email, p.Phone, p.BirthDate))
                .ToListAsync(cancellationToken);

            return customers;
        }
    }
}
