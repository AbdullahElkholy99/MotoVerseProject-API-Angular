using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Queries.Responses;
using MotoVerse.Entities.Models.Motorcycles;

namespace MotoVerse.Core.Mapping.Motorcycles;

public class MotorcycleImageProfile : Profile
{
    public MotorcycleImageProfile()
    {
        CreateMap<MotorcycleImage,
            GetMotorcycleImageListResponse>();

        CreateMap<MotorcycleImage,
            GetMotorcycleImageByIdResponse>();

        CreateMap<MotorcycleImage,
            GetMotorcycleImagePaginatedListResponse>();
    }
}