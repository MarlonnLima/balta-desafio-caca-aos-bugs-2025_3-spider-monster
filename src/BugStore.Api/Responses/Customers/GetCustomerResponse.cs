namespace BugStore.Api.Responses.Customers;

public record GetCustomerResponse(Guid Id, string Name, string Email, string Phone, DateTime BirthDate);