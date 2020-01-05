using AutoMapper;

namespace BusinessLogic
{
    public class DataMapper : Profile
    {
        public DataMapper()
        {
            CreateMap<DataAccess.Entities.User, DTO.AuthenticatedUser>();
        }
    }
}
