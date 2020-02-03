using AutoMapper;

namespace BusinessLogic
{
    public class DataMapper : Profile
    {
        public DataMapper()
        {
            CreateMap<DataAccess.Entities.User, DTO.AuthenticatedUser>();
            CreateMap<DataAccess.Entities.Mood, DTO.Mood>();
            CreateMap<DataAccess.Entities.UserMood, DTO.UserMood>().ReverseMap();
            CreateMap<DataAccess.Entities.UserMood, DTO.UserMoodDetails>()
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.MoodId, x => x.MapFrom(y => y.MoodId))
                .ForMember(x => x.Name, x => x.MapFrom(y => y.Mood.Name))
                .ForMember(x => x.RecordedOn, x => x.MapFrom(y => y.CreatedDate))
                .ForMember(x => x.Comments, x => x.MapFrom(y => y.Comments));
            CreateMap<DTO.FilterMoodParameter, DataAccess.DTO.FilterMood>();
        }
    }
}