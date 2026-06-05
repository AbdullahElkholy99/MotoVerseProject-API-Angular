namespace MotoVerse.Core.Mapping.Motorcycles;

public partial class MotorcycleProfile : Profile
{
    public MotorcycleProfile()
    {
        CreateMap<AddMotorcycleCommand, Motorcycle>();

        CreateMap<Motorcycle, GetMotorcycleListResponse>()
            .ForMember(dest => dest.ImagesPath, src => src.MapFrom(opt => opt.Images.Select(img => img.ImageUrl)));
        ;

        CreateMap<Motorcycle, GetMotorcycleByIdResponse>()
            .ForMember(dest => dest.ImagesPath, src => src.MapFrom(opt => opt.Images.Select(img => img.ImageUrl)));

        CreateMap<Motorcycle, GetMotorcyclePaginatedListResponse>()
            .ForMember(dest => dest.ImagesPath, src => src.MapFrom(opt => opt.Images.Select(img => img.ImageUrl)));
        ;

        CreateMap<EditMotorcycleCommand, Motorcycle>();
    }
}
