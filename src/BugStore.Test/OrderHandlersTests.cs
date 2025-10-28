using BugStore.Api.Commands.Customers;
using BugStore.Api.Handlers.Customers;
using BugStore.Api.Handlers.Products;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test
{
    public class OrderHandlersTests
    {
        [Fact]
        public async Task should_create_and_persist_a_order()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new CreateOrderHandler(db);

            var customerId = Guid.NewGuid();
            var product1Id = Guid.NewGuid();
            var product2Id = Guid.NewGuid();

            db.Customers.Add(new BugStore.Api.Models.Customer
            {
                Id = customerId,
                Name = "John Doe",
                Email = "john@doe.com",
                BirthDate = new DateTime(1990, 1, 1),
                Phone = "(11) 99999-9999",
            });

            db.Products.Add(new BugStore.Api.Models.Product
            {
                Id = product1Id,
                Title = "Product 1",
                Description = "Description 1",
                Slug = "product-1",
                Price = 100m,
            });

            db.Products.Add(new BugStore.Api.Models.Product
            {
                Id = product2Id,
                Title = "Product 2",
                Description = "Description 2",
                Slug = "product-2",
                Price = 50m,
            });

            await db.SaveChangesAsync();

            var request = new BugStore.Api.Requests.Orders.CreateOrderRequest(
                customerId,
                new List<Api.Requests.Orders.OrderLineRequest>
                {
                    new Api.Requests.Orders.OrderLineRequest(7, product1Id),
                    new Api.Requests.Orders.OrderLineRequest(2, product2Id),
                }
            );
            // Act

            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().NotBe(Guid.Empty);

            var saved = db.Orders
                .AsNoTracking()
                .Include(s => s.Lines)
                .FirstOrDefault(x => x.Id == response.Id);

            saved.Should().NotBeNull();
            saved.Id.Should().Be(response.Id);
            saved.CustomerId.Should().Be(request.CustomerId);
            saved.Lines.Should().HaveCount(2);
        }

        [Fact]
        public async Task should_get_a_order_by_id_when_exists()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new GetOrderByIdHandler(db);
            var orderId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var product1Id = Guid.NewGuid();
            var product2Id = Guid.NewGuid();

            db.Customers.Add(new Api.Models.Customer()
            {
                Id = customerId,
                Name = "John Doe",
                Email = "john@doe.com",
                Phone = "(51) 99999-9999",
                BirthDate = new DateTime(1900, 1, 1)
            });

            db.Products.Add(new BugStore.Api.Models.Product
            {
                Id = product1Id,
                Title = "Product 1",
                Description = "Description 1",
                Slug = "product-1",
                Price = 100m,
            });

            var order = new BugStore.Api.Models.Order
            {
                Id = orderId,
                CustomerId = customerId,
                CreatedAt = DateTime.UtcNow,
                Lines = new List<BugStore.Api.Models.OrderLine>
                {
                    new BugStore.Api.Models.OrderLine
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product1Id,
                        Quantity = 3,
                        OrderId = orderId,
                    }
                }
            };

            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            var request = new BugStore.Api.Requests.Orders.GetOrderByIdRequest(orderId);
            // Act

            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Customer.Id.Should().Be(order.CustomerId);
            response.Lines.Should().HaveCount(1);
        }
    }
}
