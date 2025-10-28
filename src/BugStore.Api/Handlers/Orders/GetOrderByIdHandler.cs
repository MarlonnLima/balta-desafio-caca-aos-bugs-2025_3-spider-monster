using BugStore.Api.Data;
using BugStore.Api.Requests.Orders;
using BugStore.Api.Responses.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Api.Handlers.Products
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdRequest, GetOrderByIdResponse>
    {
        private readonly AppDbContext _dbContext;

        public GetOrderByIdHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetOrderByIdResponse?> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .AsNoTracking()
                .Include(o => o.Customer)
                .Include(o => o.Lines)
                .ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (order == null)
                return null;

            order.Lines.ForEach(x => x.Total = (x.Quantity * x.Product.Price));

            return new GetOrderByIdResponse(order.Id, order.Customer, order.CreatedAt, order.UpdatedAt.Date, order.Lines);
        }
    }
}
