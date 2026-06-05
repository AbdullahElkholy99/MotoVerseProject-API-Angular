
using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Responses;

namespace MotoVerse.Core.Features.CategoryFeature.Queries.Models;

public record GetCategoryByIdQuery(string Id)
    : IRequest<Response<GetCategoryByIdResponse>>;
