using BugStore.Api.Commands.Customers;
using BugStore.Api.Handlers.Customers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Test
{
    public class CustomerHandlersTests
    {
        [Fact]
        public async Task should_create_and_persist_a_customer()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new CreateCustomerHandler(db);
            var request = new BugStore.Api.Requests.Customers.CreateCustomerRequest(

               "John Doe",
               "john@joe.com.br",
               "(11) 99999-9999",
               new DateTime(1990, 1, 1)
            );
            // Act

            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().NotBe(Guid.Empty);

            var saved = db.Customers
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == response.Id);

            saved.Should().NotBeNull();
            saved.Name.Should().Be(request.Name);
            saved.Email.Should().Be(request.Email);
            saved.Phone.Should().Be(request.Phone);
            saved.BirthDate.Should().Be(request.BirthDate);
        }

        [Fact]
        public async Task should_get_a_customer_when_exists()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new GetCustomerHandler(db);
            var request = new BugStore.Api.Requests.Customers.GetCustomerRequest();
            var customer = new BugStore.Api.Models.Customer
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "john@doe.com",
                Phone = "(11) 99999-9999",
                BirthDate = new DateTime(1990, 1, 1),
            };

            db.Customers.Add(customer);
            await db.SaveChangesAsync();
            // Act

            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(1);
            response.First().Id.Should().Be(customer.Id);
            response.First().Name.Should().Be(customer.Name);
            response.First().Email.Should().Be(customer.Email);
            response.First().Phone.Should().Be(customer.Phone);
        }

        [Fact]
        public async Task should_get_a_customer_by_id_when_exists()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new GetCustomerByIdHandler(db);
            var customerGuid = Guid.NewGuid();
            var customer = new BugStore.Api.Models.Customer
            {
                Id = customerGuid,
                Name = "John Doe",
                Email = "john@doe.com",
                Phone = "(11) 99999-9999",
                BirthDate = new DateTime(1990, 1, 1),
            };

            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            var request = new BugStore.Api.Requests.Customers.GetCustomerByIdRequest(customerGuid);
            // Act

            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(customer.Id);
            response.Name.Should().Be(customer.Name);
            response.Email.Should().Be(customer.Email);
            response.Phone.Should().Be(customer.Phone);
        }

        [Fact]
        public async Task should_delete_a_customer_by_id_when_exists()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new DeleteCustomerHandler(db);
            var customerGuid = Guid.NewGuid();
            var customer = new BugStore.Api.Models.Customer
            {
                Id = customerGuid,
                Name = "John Doe",
                Email = "john@doe.com",
                Phone = "(11) 99999-9999",
                BirthDate = new DateTime(1990, 1, 1),
            };

            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            var request = new BugStore.Api.Requests.Customers.DeleteCustomerRequest(customerGuid);
            // Act

            var response = await handler.Handle(request, CancellationToken.None);

            var registriesCount = db.Customers
                .AsNoTracking()
                .Count();

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(customer.Id);
            registriesCount.Should().Be(0);
        }

        [Fact]
        public async Task should_update_a_customer_when_exists()
        {
            // Arrange
            using var db = TestingDb.NewInMemory();
            var handler = new UpdateCustomerHandler(db);
            var customerGuid = Guid.NewGuid();
            var customer = new BugStore.Api.Models.Customer
            {
                Id = customerGuid,
                Name = "John Doe",
                Email = "john@doe.com",
                Phone = "(11) 99999-9999",
                BirthDate = new DateTime(1990, 1, 1),
            };

            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            var command = new UpdateCustomerCommand(customerGuid, new Api.Requests.Customers.UpdateCustomerRequest("Mariah Doe", "mariah@doe.com", "(51) 99999-9999", DateTime.MaxValue));
            // Act

            var response = await handler.Handle(command, CancellationToken.None);
            var updated = db.Customers
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == customerGuid);

            // Assert
            response.Should().NotBeNull();
            updated.Should().NotBeNull();
            updated.Name.Should().Be("Mariah Doe");
            updated.Email.Should().Be("mariah@doe.com");
            updated.Phone.Should().Be("(51) 99999-9999");
            updated.BirthDate.Should().Be(DateTime.MaxValue);
        }
    }
}
