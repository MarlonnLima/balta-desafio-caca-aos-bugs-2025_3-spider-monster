using BugStore.Api.Data;
using BugStore.Api.Models;
using BugStore.Api.Requests.Orders;
using BugStore.Api.Responses.Orders;
using MediatR;

namespace BugStore.Api.Handlers.Products
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, CreateOrderResponse>
    {
        private readonly AppDbContext _dbContext;

        public CreateOrderHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var orderGuid = Guid.NewGuid();
            var lines = new List<OrderLine>();
            
            foreach(var line in request.Lines)
            {
                lines.Add(new OrderLine()
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderGuid,
                    ProductId = line.ProductId,
                    Quantity = line.Quantity,
                });
            }

            var order = new Order
            {
                Id = orderGuid,
                CustomerId = request.CustomerId,
                Lines = lines,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateOrderResponse(order.Id);
        }
    }
}
