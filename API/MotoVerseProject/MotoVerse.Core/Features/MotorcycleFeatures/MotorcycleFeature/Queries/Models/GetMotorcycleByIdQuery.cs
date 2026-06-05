using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Responses;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Models;

public record GetMotorcycleByIdQuery(string Id)
    : IRequest<Response<GetMotorcycleByIdResponse>>;
