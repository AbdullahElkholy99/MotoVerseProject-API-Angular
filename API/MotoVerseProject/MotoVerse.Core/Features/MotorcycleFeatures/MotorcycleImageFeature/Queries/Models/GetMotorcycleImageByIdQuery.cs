using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Queries.Responses;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Queries.Models;

public record GetMotorcycleImageByIdQuery(string Id)
    : IRequest<Response<GetMotorcycleImageByIdResponse>>;
