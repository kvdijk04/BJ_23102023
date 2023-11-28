using AutoMapper;
using BJ.Contract.VisitorCounter;
using BJ.Persistence.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BJ.Application.Service
{
    public interface IVisitorCounterService
    {
        Task<VisitorCounterDto> GetVisitorCounter();

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

        public async Task<VisitorCounterDto> GetVisitorCounter()
        {
            var visitor = await _context.VisitorCounters.ToListAsync();
            var visitorDto = _mapper.Map<List<VisitorCounterDto>>(visitor);

            return visitorDto[0];
        }

        public async Task UpdateCount(UpdateVisitorCounterDto updateVisitorCounterDto)
        {

            var item = await _context.VisitorCounters.FirstOrDefaultAsync(x => x.Year == DateTime.Now.Year);

            if (item != null)
            {
                updateVisitorCounterDto.MonthCount = item.MonthCount + 1;
                updateVisitorCounterDto.DayCount = item.DayCount + 1;
                updateVisitorCounterDto.YearCount = item.YearCount + 1;

                updateVisitorCounterDto.Year = item.Year;
                updateVisitorCounterDto.Day = item.Day;
                updateVisitorCounterDto.Month = item.Month;  

                if (updateVisitorCounterDto.Day != DateTime.Now.Day)
                {
                    updateVisitorCounterDto.DayCount = 1;
                    updateVisitorCounterDto.Day = DateTime.Now.Day;
                }
                if (updateVisitorCounterDto.Month != DateTime.Now.Month)
                {
                    updateVisitorCounterDto.MonthCount = 1;
                    updateVisitorCounterDto.Month = DateTime.Now.Month;

                }
                if (updateVisitorCounterDto.Year != DateTime.Now.Year)
                {
                    updateVisitorCounterDto.YearCount = 1;
                    updateVisitorCounterDto.Year = DateTime.Now.Year;

                }

                _context.Update(_mapper.Map(updateVisitorCounterDto, item));

                await _context.SaveChangesAsync();
            }
        }
    }
}
