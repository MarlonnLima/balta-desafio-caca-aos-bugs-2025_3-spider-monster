using BugStore.Api.Commands.Products;
using BugStore.Api.Data;
using BugStore.Api.Responses.Products;
using MediatR;

namespace BugStore.Api.Handlers.Products
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResponse>
    {
        private readonly AppDbContext _dbContext;

        public UpdateProductHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateProductResponse?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _dbContext
                .Products
                .FirstOrDefault(p => p.Id == request.Id);

            if (product == null)
                return null;

            product.Title = request.Data.Title;
            product.Description = request.Data.Description;
            product.Slug = request.Data.Slug;
            product.Price = request.Data.Price;

            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateProductResponse();
        }
    }
}
