using AutoMapper;
using Building.IdentityServer.Core.Entities;

namespace Building.IdentityServer.Core.DTO
{
    public class MappingProfile:Profile
    {
        /// <summary>
        /// Hera are created profiles to map
        /// </summary>
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
        }
    }
}
