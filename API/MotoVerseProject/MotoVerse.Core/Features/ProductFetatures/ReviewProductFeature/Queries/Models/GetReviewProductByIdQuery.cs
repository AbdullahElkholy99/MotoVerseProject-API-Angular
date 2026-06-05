using MotoVerse.Core.Features.ReviewProductFeature.Queries.Responses;

namespace MotoVerse.Core.Features.ReviewProductFeature.Queries.Models;

public record GetReviewProductByIdQuery(string Id)
    : IRequest<Response<GetReviewProductByIdResponse>>;
