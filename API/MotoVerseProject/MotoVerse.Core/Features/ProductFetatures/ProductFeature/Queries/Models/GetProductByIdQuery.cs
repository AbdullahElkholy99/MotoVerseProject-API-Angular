namespace MotoVerse.Core.Features.ProductFeature.Queries.Models;

public record GetProductByIdQuery(string Id)
    : IRequest<Response<GetProductByIdResponse>>;
