using BugStore.Api.Data;
using BugStore.Api.Models;
using BugStore.Api.Requests.Products;
using BugStore.Api.Responses.Products;
using MediatR;

namespace BugStore.Api.Handlers.Products
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequest, CreateProductResponse>
    {
        private readonly AppDbContext _dbContext;

        public CreateProductHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreateProductResponse> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Slug = request.Slug,
                Price = request.Price
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateProductResponse(product.Id);
        }
    }
}
