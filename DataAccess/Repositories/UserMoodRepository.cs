using DataAccess.DTO;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserMoodRepository : Repository<UserMood>, IUserMoodRepository
    {
        public UserMoodRepository(AppDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserMood>> GetMoodsForUser(FilterMood parameters)
        {
            var filteredUserMoods = GetQueryable()
                .Include(x => x.User)
                .Include(x => x.Mood)
                .Where(x => x.UserId == parameters.UserId);

            if (parameters.From != null && parameters.To != null)
            {
                filteredUserMoods = filteredUserMoods.Where(x => x.CreatedDate >= parameters.From && x.CreatedDate <= parameters.To);
            }
            else if (parameters.From != null)
            {
                filteredUserMoods = filteredUserMoods.Where(x => x.CreatedDate >= parameters.From);
            }
            else if (parameters.To != null)
            {
                filteredUserMoods = filteredUserMoods.Where(x => x.CreatedDate <= parameters.To);
            }

            return await filteredUserMoods.ToListAsync();
        }
    }
}