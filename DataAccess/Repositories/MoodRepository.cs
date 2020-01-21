using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public class MoodRepository : Repository<Mood>, IMoodRepository
    {
        public MoodRepository(AppDBContext dbContext) : base(dbContext)
        {
        }
    }
}