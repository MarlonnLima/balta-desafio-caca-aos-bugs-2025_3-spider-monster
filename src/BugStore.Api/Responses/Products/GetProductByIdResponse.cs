namespace BugStore.Api.Responses.Products;

public record GetProductByIdResponse(Guid Id, string Title, string Description, string Slug, decimal Price);