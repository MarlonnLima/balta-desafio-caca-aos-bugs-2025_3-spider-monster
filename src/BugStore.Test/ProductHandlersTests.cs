using BugStore.Api.Commands.Products;
using BugStore.Api.Handlers.Products;
using BugStore.Api.Requests.Products;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test
{
    public class ProductHandlersTests
    {
        [Fact]
        public async Task should_create_and_persist_a_product()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new CreateProductHandler(db);
            var request = new CreateProductRequest(
                "Caneca Balta.io",
                "Caneca personalizada do Balta.io",
                "caneca-balta-io",
                39.90m
            );
            // Act

            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().NotBe(Guid.Empty);

            var saved = db.Products
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == response.Id);

            saved.Should().NotBeNull();
            saved.Should().BeEquivalentTo(new BugStore.Api.Models.Product
            {
                Id = response.Id,
                Title = request.Title,
                Description = request.Description,
                Slug = request.Slug,
                Price = request.Price,
            });
        }

        [Fact]
        public async Task should_get_a_product_when_exists()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new GetProductHandler(db);
            var request = new GetProductRequest();
            var product = new BugStore.Api.Models.Product
            {
                Id = Guid.NewGuid(),
                Title = "Caneca Balta.io",
                Description = "Caneca personalizada do Balta.io",
                Slug = "caneca-balta-io",
                Price = 39.90m
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();
            // Act

            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);
            response.First().Should().BeEquivalentTo(new BugStore.Api.Responses.Products.GetProductResponse(
                product.Id,
                product.Title,
                product.Description,
                product.Slug,
                product.Price
            ));
        }

        [Fact]
        public async Task should_get_a_product_by_id_when_exists()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new GetProductByIdHandler(db);
            var productGuid = Guid.NewGuid();
            var product = new BugStore.Api.Models.Product
            {
                Id = productGuid,
                Title = "Caneca Balta.io",
                Description = "Caneca personalizada do Balta.io",
                Slug = "caneca-balta-io",
                Price = 39.90m
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            var request = new BugStore.Api.Requests.Products.GetProductByIdRequest(productGuid);
            // Act

            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(new BugStore.Api.Responses.Products.GetProductByIdResponse(
                product.Id,
                product.Title,
                product.Description,
                product.Slug,
                product.Price
            ));
        }

        [Fact]
        public async Task should_delete_a_product_by_id_when_exists()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new DeleteProductHandler(db);
            var productGuid = Guid.NewGuid();
            var product = new BugStore.Api.Models.Product
            {
                Id = productGuid,
                Title = "Caneca Balta.io",
                Description = "Caneca personalizada do Balta.io",
                Slug = "caneca-balta-io",
                Price = 39.90m
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            var request = new DeleteProductRequest(productGuid);
            // Act

            var response = await handler.Handle(request, CancellationToken.None);

            var registriesCount = db.Customers
                .AsNoTracking()
                .Count();

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(product.Id);
            registriesCount.Should().Be(0);
        }

        [Fact]
        public async Task should_update_a_product_when_exists()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new UpdateProductHandler(db);
            var productGuid = Guid.NewGuid();
            var product = new BugStore.Api.Models.Product
            {
                Id = productGuid,
                Title = "Caneca Balta.io",
                Description = "Caneca personalizada do Balta.io",
                Slug = "caneca-balta-io",
                Price = 39.90m
            };

            db.Products.Add(product);
            await db.SaveChangesAsync();

            var command = new UpdateProductCommand(productGuid, new UpdateProductRequest("Caneca Marlon", "Caneca Personalizada do Marlon", "caneca-marlon", 29.90m));
            // Act

            var response = await handler.Handle(command, CancellationToken.None);
            var updated = db.Products
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == productGuid);

            // Assert
            response.Should().NotBeNull();
            updated.Should().NotBeNull();
            updated.Should().BeEquivalentTo(new BugStore.Api.Models.Product
            {
                Id = productGuid,
                Title = command.Data.Title,
                Description = command.Data.Description,
                Slug = command.Data.Slug,
                Price = command.Data.Price,
            });
        }
    }
}
