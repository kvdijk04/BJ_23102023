using AutoMapper;
using BJ.Contract.VisitorCounter;
using BJ.Persistence.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface IVisitorCounterService
    {
        Task<long> GetVisitorCounter();

        Task UpdateCount(UpdateVisitorCounterDto updateVisitorCounterDto);
    }
    public class VisitorCounterService : IVisitorCounterService
    {
        private readonly BJContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public VisitorCounterService(BJContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<long> GetVisitorCounter()
        {
            var visitor = await _context.VisitorCounters.ToListAsync();
            var visitorDto = _mapper.Map<List<VisitorCounterDto>>(visitor);

            return visitorDto[0].Count;
        }

        public async Task UpdateCount(UpdateVisitorCounterDto updateVisitorCounterDto)
        {

            var item = await _context.VisitorCounters.ToListAsync();

            if (item[0] != null)
            {
                updateVisitorCounterDto.Count = item[0].Count + 1;
                updateVisitorCounterDto.Year = item[0].Year;
                if (updateVisitorCounterDto.Year != DateTime.Now.Year)
                {
                    updateVisitorCounterDto.Count = 1;
                    updateVisitorCounterDto.Year = DateTime.Now.Year;
                }

                _context.Update(_mapper.Map(updateVisitorCounterDto, item[0]));

                await _context.SaveChangesAsync();
            }
        }
    }
}
