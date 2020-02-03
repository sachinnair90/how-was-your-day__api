using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess;
using DataAccess.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class MoodService : IMoodService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public MoodService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Mood>> GetAllMoodsAsync()
        {
            return mapper.Map<IEnumerable<DataAccess.Entities.Mood>, IEnumerable<Mood>>(await uow.MoodRepository.GetAllAsync());
        }

        public async Task<bool> AddMoodForUser(UserMood userMood)
        {
            var moodEntity = mapper.Map<UserMood, DataAccess.Entities.UserMood>(userMood);

            uow.UserMoodRepository.Add(moodEntity);

            return (await uow.SaveChangesAsync()) > 0;
        }

        public async Task<IEnumerable<UserMoodDetails>> GetMoodsForUser(FilterMoodParameter moodFilter)
        {
            var parameter = mapper.Map<FilterMoodParameter, FilterMood>(moodFilter);

            return mapper.Map<IEnumerable<DataAccess.Entities.UserMood>, IEnumerable<UserMoodDetails>>(await uow.UserMoodRepository.GetMoodsForUser(parameter));
        }
    }
}