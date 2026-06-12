using MotoVerse.Core.Features.Authorization.Quaries.Results;

namespace MotoVerse.Core.Mapping.Roles
{
    public partial class RoleProfile
    {
        public void GetRolesListMapping()
        {
            CreateMap<MyRole, GetRolesListResult>();
        }
    }
}
