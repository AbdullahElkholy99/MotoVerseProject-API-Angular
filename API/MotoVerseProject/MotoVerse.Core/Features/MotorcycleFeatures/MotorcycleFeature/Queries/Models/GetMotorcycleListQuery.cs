
namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Models;

public record GetMotorcycleListQuery
    : IRequest<Response<List<GetMotorcycleListResponse>>>;