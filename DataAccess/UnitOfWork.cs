using DataAccess.Repositories;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;
        public IUserRepository UserRepository { get; }
        public IMoodRepository MoodRepository { get; }

        public UnitOfWork(AppDBContext context, IUserRepository userRepository, IMoodRepository moodRepository)
        {
            _context = context;
            UserRepository = userRepository;
            MoodRepository = moodRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
