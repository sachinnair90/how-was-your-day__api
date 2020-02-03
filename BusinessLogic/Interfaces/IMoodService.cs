using BusinessLogic.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMoodService
    {
        public Task<IEnumerable<Mood>> GetAllMoodsAsync();

        Task<bool> AddMoodForUser(UserMood userMood);

        Task<IEnumerable<UserMoodDetails>> GetMoodsForUser(FilterMoodParameter moodFilter);
    }
}