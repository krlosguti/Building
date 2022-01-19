using AutoMapper;
using Building.IdentityServer.Core.Entities;

namespace Building.IdentityServer.Core.DTO
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
        }
    }
}
