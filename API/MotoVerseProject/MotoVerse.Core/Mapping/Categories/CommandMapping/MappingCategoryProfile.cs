using MotoVerse.Core.Features.CategoryFeature.Commands.Models;
using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Queries.Responses;

namespace MotoVerse.Core.Mapping.Categories.CommandMapping;

public partial class MappingCategoryProfile : Profile
{
    public MappingCategoryProfile()
    {
        CreateMap<Category, GetCategoryListResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
            .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count()));

        CreateMap<Category, GetCategoryPaginatedListResponse>()
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
          .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(src => src.Products.Count()));

        CreateMap<AddCategoryCommand, Category>();
        CreateMap<EditCategoryCommand, Category>();


    }
}
