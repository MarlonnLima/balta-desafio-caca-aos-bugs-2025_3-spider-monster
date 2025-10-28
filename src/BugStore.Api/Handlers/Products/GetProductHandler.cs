using BugStore.Api.Data;
using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Api.Handlers.Products
{
    public class GetProductHandler : IRequestHandler<GetProductRequest, IEnumerable<GetProductResponse>>
    {
        private readonly AppDbContext _dbContext;

        public GetProductHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GetProductResponse>> Handle(GetProductRequest request, CancellationToken cancellationToken)
        {
            var products = await _dbContext.Products
                .AsNoTracking()
                .Select(p => new GetProductResponse(p.Id, p.Title, p.Description, p.Slug, p.Price))
                .ToListAsync(cancellationToken);

            return products;
        }
    }
}
