namespace MotoVerse.Core.Mapping.Products;

public class ProductProfile : Profile
{
    public ProductProfile()
    {

        CreateMap<Product, GetProductByIdResponse>()
                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Localize(src.Category.NameAr, src.Category.NameEn)));

        CreateMap<Product, GetProductListResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Localize(src.Category.NameAr, src.Category.NameEn)));


        CreateMap<Product, GetProductPaginatedListResponse>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Localize(src.NameAr, src.NameEn)))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Localize(src.Category.NameAr, src.Category.NameEn)));

        CreateMap<AddProductCommand, Product>();
        CreateMap<EditProductCommand, Product>();
    }

}
