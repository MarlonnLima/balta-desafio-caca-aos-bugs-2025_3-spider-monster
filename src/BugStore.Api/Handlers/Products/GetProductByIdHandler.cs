using BugStore.Api.Data;
using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Products;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Api.Handlers.Products
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdRequest, GetProductByIdResponse>
    {
        private readonly AppDbContext _dbContext;

        public GetProductByIdHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetProductByIdResponse?> Handle(GetProductByIdRequest request, CancellationToken cancellationToken)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
                return null;


            return new GetProductByIdResponse(product.Id, product.Title, product.Description, product.Slug, product.Price);
        }
    }
}
