using MotoVerse.Entities.Models.Users;

namespace MotoVerse.Core.Mapping.Products;

public partial class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddUserCommand, Customer>()
            .ForMember(des => des.UserName, src => src.MapFrom(opt => opt.Email));
    }
}
