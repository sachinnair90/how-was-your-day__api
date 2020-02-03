using DataAccess.DTO;
using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IUserMoodRepository : IRepository<UserMood>
    {
        Task<IEnumerable<UserMood>> GetMoodsForUser(FilterMood parameters);
    }
}