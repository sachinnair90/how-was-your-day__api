using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.Interfaces;
using DataAccess;
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
    }
}