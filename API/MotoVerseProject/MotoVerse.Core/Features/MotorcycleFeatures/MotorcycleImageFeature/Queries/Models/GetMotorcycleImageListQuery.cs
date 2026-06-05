using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Queries.Responses;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Queries.Models;

public record GetMotorcycleImageListQuery
    : IRequest<Response<List<GetMotorcycleImageListResponse>>>;
